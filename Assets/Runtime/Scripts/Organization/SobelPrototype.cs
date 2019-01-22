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
		
		ParallelConvolutionSequences<float> m_ParallelConvolutionSequences;
		ParallelJobSequences<FloatWithFloatConvolveJob> m_JobSequences;

		FloatWithFloatConvolveJob[][] m_ParallelJobSequences = new FloatWithFloatConvolveJob[2][];

		FloatParallelConvolutionJobSequence m_NewSequence;

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
			
			m_ParallelConvolutionSequences = new ParallelConvolutionSequences<float>(new [] 
				{ convolutionOne, convolutionTwo });
		}
		
		void SetupJobs()
		{
			m_PadJob = new ImagePadJob<float>(m_InputData, m_PaddedGrayscaleInputData, m_Pad);
			m_PadJob.Run();
			
			// TODO - figure out if we can make this generic
			m_JobSequences = new ParallelJobSequences<FloatWithFloatConvolveJob>
				(m_ParallelConvolutionSequences.Width, 1);
			
			m_NewSequence = new FloatParallelConvolutionJobSequence(m_PadJob.Output,
				m_ParallelConvolutionSequences, m_JobSequences);
			
			m_NewSequence.InitializeJobs();
			
			m_CombineJob = new SquareCombineJob()
			{
				A = m_NewSequence.Jobs[0][0].Output,
				B = m_JobSequences[1][0].Output,
				Output = m_CombinedConvolutionData,
			};
		}
		
		public JobHandle Schedule(JobHandle dependency)
		{
			var handle = m_PadJob.Schedule(dependency);
			handle = m_NewSequence.Schedule(handle);
			//handle =  m_ParallelJobSequences.ScheduleParallel(handle);
			handle = m_CombineJob.Schedule(m_ConvolvedDataOne.Buffer.Length, 2048, handle);
			m_JobHandle = handle;
			return handle;
		}
		
		public void Complete()
		{
			m_JobHandle.Complete();
		}

		public void OnJobsComplete()
		{
			Debug.Log("jobs complete event");
			// TODO - extension method for texture that loads an ImageData
			ConvolvedTextureOne.LoadRawTextureData(m_NewSequence.Images[0].Buffer);
			ConvolvedTextureOne.Apply();
			ConvolvedTextureTwo.LoadRawTextureData(m_NewSequence.Images[1].Buffer);
			ConvolvedTextureTwo.Apply();
			ConvolutionOutputTexture.LoadRawTextureData(m_CombineJob.Output.Buffer);
			ConvolutionOutputTexture.Apply();
		}

		void SetupTextures(Texture2D input)
		{
			m_InputData = new ImageData<float>(input);

			m_Pad = Pad.GetSamePad(m_InputData, m_ParallelConvolutionSequences.Sequences[0].Convolutions[0]);
			var newSize = Pad.GetNewSize(m_InputData.Width, m_InputData.Height, m_Pad);
			m_PaddedGrayscaleInputData = new ImageData<float>(newSize.x, newSize.y, Allocator.Persistent);

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
			m_ParallelConvolutionSequences.Dispose();
		}
	}
}
