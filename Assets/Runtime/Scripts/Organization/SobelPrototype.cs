using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
	public class SobelFloatPrototype : IDisposable
	{
		public Texture2D ConvolvedTextureOne;
		public Texture2D ConvolvedTextureTwo;
		public Texture2D ConvolutionOutputTexture;

		ImageData<float> m_InputData;
		ImageData<float> m_PaddedGrayscaleInputData;
		ImageData<float> m_ConvolvedDataOne;
		ImageData<float> m_ConvolvedDataTwo;
		ImageData<float> m_CombinedConvolutionData;
	
		JobHandle m_GrayScaleJobHandle;
		JobHandle m_JobHandle;
		
		ParallelConvolutions<float> m_ParallelConvolutions;

		FloatWithFloatConvolveJob[][] m_ParallelJobSequences = new FloatWithFloatConvolveJob[2][];

		SquareCombineJob m_CombineJob;

		ImagePadJob<float> m_PadJob;
		
		Padding m_Pad;

		public SobelFloatPrototype(Texture2D input)
		{
			SetupFilter();
			SetupTextures(input);
			SetupJobs();

			Debug.Log("sobel constructor done");
		}
		void SetupFilter()
		{
			// TODO - keep this example and make a separated one to compare
			var kernelOne = new Kernel<float>(Kernels.Short.Sobel.X.ToFloat());
			var kernelTwo = new Kernel<float>(Kernels.Short.Sobel.Y.ToFloat());
			var convolutionOne = new ConvolutionSequence<float>(new Convolution<float>(kernelOne));
			var convolutionTwo = new ConvolutionSequence<float>(new Convolution<float>(kernelTwo));
			
			m_ParallelConvolutions = new ParallelConvolutions<float>(new [] 
				{ convolutionOne, convolutionTwo });
		}
		
		void SetupJobs()
		{
			// TODO - figure out if we can make this generic
			var sequenceOne = m_ParallelConvolutions.Sequences[0];
			var jobs = new FloatWithFloatConvolveJob[1];

			m_PadJob = new ImagePadJob<float>(m_InputData, m_PaddedGrayscaleInputData, m_Pad);
			
			for (var j = 0; j < jobs.Length; j++)
			{
				jobs[j] = new FloatWithFloatConvolveJob(sequenceOne.Convolutions[0],
					m_PadJob.Output, m_ConvolvedDataOne);
			}
			
			var sequenceTwo = m_ParallelConvolutions.Sequences[1];
			var jobsTwo = new FloatWithFloatConvolveJob[1];
			for (var j = 0; j < jobs.Length; j++)
			{
				jobsTwo[j] = new FloatWithFloatConvolveJob(sequenceTwo.Convolutions[0],
					m_PadJob.Output, m_ConvolvedDataTwo);
			}
			
			m_ParallelJobSequences[0] = jobs;
			m_ParallelJobSequences[1] = jobsTwo;

			m_CombineJob = new SquareCombineJob()
			{
				A = jobs[0].Output,
				B = jobsTwo[0].Output,
				Output = m_CombinedConvolutionData,
			};
			
			m_MinMaxJob = new FindMinMaxJob(m_CombineJob.Output.Buffer);
			m_NormalizeJob = new ImageNormalize01Job(m_MinMaxJob.Data, m_MinMaxJob.MinMaxOutput);
		}

		FindMinMaxJob m_MinMaxJob;
		ImageNormalize01Job m_NormalizeJob;
		
		public JobHandle Schedule(JobHandle dependency)
		{
			var handle = m_PadJob.Schedule(dependency);
			handle =  m_ParallelJobSequences.ScheduleParallel(handle);
			handle = m_CombineJob.Schedule(m_ConvolvedDataOne.Buffer.Length, 2048, handle);
			handle = m_MinMaxJob.Schedule(handle);
			m_JobHandle = handle;
			return handle;
		}
		
		public JobHandle ScheduleNormalize(JobHandle dependency)
		{
			m_NormalizeJob = new ImageNormalize01Job(m_MinMaxJob.Data, m_MinMaxJob.MinMaxOutput);
			m_JobHandle = m_NormalizeJob.Schedule(m_NormalizeJob.Data.Length, 2048, dependency);
			return m_JobHandle;
		}
		
		public void Complete()
		{
			m_JobHandle.Complete();
		}

		public void OnJobsComplete()
		{
			m_NormalizeJob = new ImageNormalize01Job(m_MinMaxJob.Data, m_MinMaxJob.MinMaxOutput);
			
			// TODO - extension method for texture that loads an ImageData
			ConvolvedTextureOne.LoadRawTextureData(m_ParallelJobSequences[0][0].Output.Buffer);
			ConvolvedTextureOne.Apply();
			ConvolvedTextureTwo.LoadRawTextureData(m_ParallelJobSequences[1][0].Output.Buffer);
			ConvolvedTextureTwo.Apply();
			var combined = m_NormalizeJob.Data;
			ConvolutionOutputTexture.LoadRawTextureData(combined);
			ConvolutionOutputTexture.Apply();
		}

		void SetupTextures(Texture2D input)
		{
			m_InputData = new ImageData<float>(input);

			m_Pad = Pad.GetSamePad(m_InputData, m_ParallelConvolutions.Sequences[0].Convolutions[0]);
			var newSize = Pad.GetNewSize(m_InputData.Width, m_InputData.Height, m_Pad);
			m_PaddedGrayscaleInputData = new ImageData<float>(newSize.x, newSize.y, Allocator.TempJob);

			ConvolvedTextureOne = SetupTexture(input, out m_ConvolvedDataOne);
			ConvolvedTextureTwo = SetupTexture(input, out m_ConvolvedDataTwo);
			ConvolutionOutputTexture = SetupTexture(input, out m_CombinedConvolutionData);
		}

		Texture2D SetupTexture<T>(Texture2D input, out ImageData<T> data)
			where T: struct
		{
			var texture = new Texture2D(input.width, input.height, TextureFormat.RFloat, false);
			data = new ImageData<T>(texture);
			return texture;
		}

		public void Dispose()
		{
			m_ParallelConvolutions.Dispose();
		}
	}
}
