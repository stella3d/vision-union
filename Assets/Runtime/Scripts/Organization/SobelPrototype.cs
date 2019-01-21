using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
	public class SobelFloatPrototype : IDisposable
	{
		public Texture2D GrayscaleInputTexture;
		public Texture2D ConvolvedTextureOne;
		public Texture2D ConvolvedTextureTwo;
		public Texture2D ConvolutionOutputTexture;

		ImageData<float> m_GrayscaleInputData;
		ImageData<float> m_ConvolvedDataOne;
		ImageData<float> m_ConvolvedDataTwo;
		ImageData<float> m_CombinedConvolutionData;
	
		JobHandle m_GrayScaleJobHandle;
		JobHandle m_JobHandle;
		
		ParallelConvolutions<float> m_ParallelConvolutions;

		FloatWithFloatConvolveJob[][] m_ParallelJobSequences = new FloatWithFloatConvolveJob[2][];

		public SobelFloatPrototype(Texture2D input)
		{
			SetupFilter();
			SetupTextures(input);
			SetupJobs();

			Debug.Log("sobel constructor done");
		}
		void SetupFilter()
		{
			var kernelOne = new Kernel<float>(Kernels.Short.Sobel.X.ToFloat());
			var kernelTwo = new Kernel<float>(Kernels.Short.Sobel.Y.ToFloat());
			var convolutionOne = new ConvolutionSequence<float>(new Convolution<float>(kernelOne));
			var convolutionTwo = new ConvolutionSequence<float>(new Convolution<float>(kernelTwo));
			
			m_ParallelConvolutions = new ParallelConvolutions<float>(new [] 
				{ convolutionOne, convolutionTwo });
		}
		
		void SetupJobs()
		{
			var sequenceOne = m_ParallelConvolutions.Sequences[0];
			var jobs = new FloatWithFloatConvolveJob[1];
			for (var j = 0; j < jobs.Length; j++)
			{
				jobs[j] = new FloatWithFloatConvolveJob()
				{
					Convolution = sequenceOne.Convolutions[0],
					Input = m_GrayscaleInputData,
					Output = m_ConvolvedDataOne
				};
			}
			
			m_ParallelJobSequences[0] = jobs;
			
			var sequenceTwo = m_ParallelConvolutions.Sequences[1];
			var jobsTwo = new FloatWithFloatConvolveJob[1];
			for (var j = 0; j < jobs.Length; j++)
			{
				jobsTwo[j] = new FloatWithFloatConvolveJob()
				{
					Convolution = sequenceTwo.Convolutions[0],
					Input = m_GrayscaleInputData,
					Output = m_ConvolvedDataOne
				};
			}
			
			m_ParallelJobSequences[1] = jobsTwo;
		}

		void SetupTextures(Texture2D input)
		{
			var inputData = new ImageData<float>(input);
			var paddedInput = Pad.ConvolutionInput(inputData, m_ParallelConvolutions);
			
			GrayscaleInputTexture = input;
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
