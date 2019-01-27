using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
	public class SobelFloatPrototype :  IDisposable
	{
		public Texture2D ConvolvedTextureOne;
		public Texture2D ConvolvedTextureTwo;
		public Texture2D ConvolutionOutputTexture;

		ImageData<float> m_InputData;
		ImageData<float> m_PaddedGrayscaleInputData;
		ImageData<float> m_ConvDataOne;
		ImageData<float> m_ConvDataTwo;
		ImageData<float> m_CombinedConvolutionData;
	
		JobHandle m_GrayScaleJobHandle;
		JobHandle m_JobHandle;
		
		ParallelConvolutions<float> m_ParallelConvolutions;
		ParallelConvolutionData<float> m_ParallelConvolutionData;

		FloatParallelConvolutionJobs m_NewSequence;

		SquareCombineJob m_CombineJob;
		ImagePadJob<float> m_PadJob;
		
		Padding m_Pad;

		public SobelFloatPrototype(Texture2D input)
		{
			m_InputData = new ImageData<float>(input);
			Setup();
		}

		public SobelFloatPrototype(ImageData<float> input)
		{
			m_InputData = input;
			Setup();
		}
		
		void Setup()
		{
			SetupFilter();
			SetupTextures(m_InputData);
			SetupJobs();
		}
		
		void SetupFilter()
		{
			m_ParallelConvolutions = new ParallelConvolutions<float>(new []
			{
				new ConvolutionSequence<float>(Kernels.Short.Sobel.X.ToFloat()), 
				new ConvolutionSequence<float>(Kernels.Short.Sobel.Y.ToFloat())
			});
		}
		
		void SetupJobs()
		{
			m_ParallelConvolutionData = new ParallelConvolutionData<float>(m_InputData, m_ParallelConvolutions);

			m_PadJob = new ImagePadJob<float>(m_InputData, m_PaddedGrayscaleInputData, m_Pad);
			m_JobHandle = m_PadJob.Schedule();
			m_JobHandle.Complete();
			
			m_NewSequence = new FloatParallelConvolutionJobs(m_PadJob.Output, m_ParallelConvolutionData , m_JobHandle);

			var outImages = m_ParallelConvolutionData.OutputImages[0][0];
			
			m_CombineJob = new SquareCombineJob()
			{
				A = m_ParallelConvolutionData.OutputImages[0][0],
				B = m_ParallelConvolutionData.OutputImages[0][1],
				Output = m_CombinedConvolutionData,
			};
			
		}
		
		public JobHandle Schedule(JobHandle dependency)
		{
			var handle = m_PadJob.Schedule(dependency);
			handle.Complete();
			handle = m_NewSequence.Schedule(handle);
			handle = m_CombineJob.Schedule(m_ParallelConvolutionData.OutputImages[0][0].Buffer.Length, 2048, handle);
			m_JobHandle = handle;
			return handle;
		}
		
		public void Complete()
		{
			m_JobHandle.Complete();
		}

		public void OnJobsComplete()
		{
			ConvolvedTextureOne.LoadImageData(m_ParallelConvolutionData.OutputImages[0][0]);
			ConvolvedTextureTwo.LoadImageData(m_ParallelConvolutionData.OutputImages[0][1]);
			ConvolutionOutputTexture.LoadImageData(m_CombineJob.Output);
		}

		void SetupTextures(Texture2D input)
		{
			SetupTextures(m_InputData);
		}
		
		void SetupTextures(ImageData<float> input)
		{
			m_Pad = Pad.GetSamePad(m_InputData, m_ParallelConvolutions[0][0]);
			var newSize = Pad.GetNewSize(m_InputData.Width, m_InputData.Height, m_Pad);
			m_PaddedGrayscaleInputData = new ImageData<float>(newSize.x, newSize.y);

			ConvolvedTextureOne = SetupTexture(input, out m_ConvDataOne);
			ConvolvedTextureTwo = SetupTexture(input, out m_ConvDataTwo);
			ConvolutionOutputTexture = SetupTexture(input, out m_CombinedConvolutionData);
		}

		Texture2D SetupTexture(Texture2D input, out ImageData<float> data)
		{
			var texture = new Texture2D(input.width, input.height, TextureFormat.RFloat, false);
			data = new ImageData<float>(texture);
			return texture;
		}
		
		Texture2D SetupTexture(ImageData<float> input, out ImageData<float> data)
		{
			var texture = new Texture2D(input.Width, input.Height, TextureFormat.RFloat, false);
			data = new ImageData<float>(texture);
			return texture;
		}

		public void Dispose()
		{
			m_PaddedGrayscaleInputData.Dispose();
			//m_ParallelConvolutions.Dispose();
			m_ParallelConvolutionData.Dispose();
			m_NewSequence.Dispose();
		}
	}
}
