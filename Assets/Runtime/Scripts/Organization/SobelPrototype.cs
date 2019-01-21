using System.Linq;
using Unity.Jobs;
using UnityEngine;
using VisionUnion.Jobs;
using VisionUnion.Organization;

namespace VisionUnion.Organization
{
	public class SobelFloatPrototype
	{
		Texture2D m_InputTexture;
		Texture2D m_GrayscaleInputTexture;
		Texture2D m_ConvolvedTextureOne;
		Texture2D m_ConvolvedTextureTwo;
		Texture2D m_ConvolutionOutputTexture;

		ImageData<float> m_GrayscaleInputData;
		ImageData<float> m_ConvolvedDataOne;
		ImageData<float> m_ConvolvedDataTwo;
		ImageData<float> m_CombinedConvolutionData;
	
		JobHandle m_GrayScaleJobHandle;
		JobHandle m_JobHandle;

		Kernel<float> m_KernelOne;
		Kernel<float> m_KernelTwo;
		
		ParallelConvolutions<float> m_ParallelConvolutions;

		FloatWithFloatConvolveJob[][] m_ParallelJobSequences = new FloatWithFloatConvolveJob[2][];

		public SobelFloatPrototype(Texture2D input)
		{
			SetupTextures(input);
			SetupFilter();
			SetupJobs();

			Debug.Log("sobel constructor done");
		}
		void SetupFilter()
		{
			m_KernelOne = new Kernel<float>(Kernels.Short.Sobel.X.ToFloat());
			m_KernelTwo = new Kernel<float>(Kernels.Short.Sobel.Y.ToFloat());
			var convolutionOne = new ConvolutionSequence<float>(new Convolution<float>(m_KernelOne));
			var convolutionTwo = new ConvolutionSequence<float>(new Convolution<float>(m_KernelTwo));
			
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
			m_GrayscaleInputTexture = SetupTexture(input, out m_GrayscaleInputData);
			m_ConvolvedTextureOne = SetupTexture(input, out m_ConvolvedDataOne);
			m_ConvolvedTextureTwo = SetupTexture(input, out m_ConvolvedDataTwo);
			m_ConvolutionOutputTexture = SetupTexture(input, out m_CombinedConvolutionData);
		}

		Texture2D SetupTexture<T>(Texture2D input, out ImageData<T> data)
			where T: struct
		{
			var texture = new Texture2D(input.width, input.height, TextureFormat.RFloat, false);
			data = new ImageData<T>(texture);
			return texture;
		}

		void OnDestroy()
		{
			m_KernelOne.Dispose();
			m_KernelTwo.Dispose();
			m_ParallelConvolutions.Dispose();
		}

		void Update ()
		{
			if (Time.frameCount < 10)
				return;
		}
	}
}
