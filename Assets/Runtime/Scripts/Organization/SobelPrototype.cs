using System;
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

		Image<float> m_Input;
		Image<float> m_PaddedGrayscaleInput;
		Image<float> m_ConvOne;
		Image<float> m_ConvTwo;
		Image<float> m_CombinedConvolution;
	
		JobHandle m_GrayScaleJobHandle;
		JobHandle m_JobHandle;
		
		ParallelConvolutions<float> m_ParallelConvolutions;
		ParallelConvolutionData<float> m_ParallelConvolutionData;

		FloatParallelConvolutionJobs m_NewSequence;

		SquareCombineJobFloat m_CombineJobFloat;
		ImagePadJob<float> m_PadJob;
		
		Padding m_Pad;

		public SobelFloatPrototype(Texture2D input)
		{
			m_Input = new Image<float>(input);
			Setup();
		}

		public SobelFloatPrototype(Image<float> input)
		{
			m_Input = input;
			Setup();
		}
		
		void Setup()
		{
			SetupFilter();
			SetupTextures(m_Input);
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
			m_ParallelConvolutionData = new ParallelConvolutionData<float>(m_Input, m_ParallelConvolutions);

			m_PadJob = new ImagePadJob<float>(m_Input, m_PaddedGrayscaleInput, m_Pad);
			m_JobHandle = m_PadJob.Schedule();
			m_JobHandle.Complete();
			
			m_NewSequence = new FloatParallelConvolutionJobs(m_PadJob.Output, m_ParallelConvolutionData , m_JobHandle);

			var outImages = m_ParallelConvolutionData.OutputImages[0];
			
			m_CombineJobFloat = new SquareCombineJobFloat()
			{
				A = outImages[0],
				B = outImages[1],
				Output = m_CombinedConvolution,
			};
			
		}
		
		public JobHandle Schedule(JobHandle dependency)
		{
			var handle = m_PadJob.Schedule(dependency);
			handle.Complete();
			handle = m_NewSequence.Schedule(handle);
			handle = m_CombineJobFloat.Schedule(m_ParallelConvolutionData.OutputImages[0][0].Buffer.Length, 2048, handle);
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
			ConvolutionOutputTexture.LoadImageData(m_CombineJobFloat.Output);
		}

		void SetupTextures(Texture2D input)
		{
			SetupTextures(m_Input);
		}
		
		void SetupTextures(Image<float> input)
		{
			m_Pad = Pad.GetSamePad(m_Input, m_ParallelConvolutions[0][0]);
			var newSize = Pad.GetNewSize(m_Input.Width, m_Input.Height, m_Pad);
			m_PaddedGrayscaleInput = new Image<float>(newSize.x, newSize.y);

			ConvolvedTextureOne = SetupTexture(input, out m_ConvOne);
			ConvolvedTextureTwo = SetupTexture(input, out m_ConvTwo);
			ConvolutionOutputTexture = SetupTexture(input, out m_CombinedConvolution);
		}

		Texture2D SetupTexture(Texture2D input, out Image<float> data)
		{
			var texture = new Texture2D(input.width, input.height, TextureFormat.RFloat, false);
			data = new Image<float>(texture);
			return texture;
		}
		
		Texture2D SetupTexture(Image<float> input, out Image<float> data)
		{
			var texture = new Texture2D(input.Width, input.Height, TextureFormat.RFloat, false);
			data = new Image<float>(texture);
			return texture;
		}

		public void Dispose()
		{
			m_PaddedGrayscaleInput.Dispose();
			m_ParallelConvolutionData.Dispose();
			m_NewSequence.Dispose();
		}
	}
}
