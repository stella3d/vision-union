using System.Linq;
using Unity.Jobs;
using UnityEngine;
using VisionUnion.Jobs;
using VisionUnion.Organization;

namespace VisionUnion.Examples
{
	public class SobelKernelExample : MonoBehaviour
	{
		[SerializeField]
		Texture2D m_InputTexture;
	
		[SerializeField]
		MeshRenderer m_ColorInputRenderer;
		[SerializeField]
		MeshRenderer m_GrayscaleRenderer;
		[SerializeField]
		MeshRenderer m_KernelOneRenderer;
		[SerializeField]
		MeshRenderer m_KernelTwoRenderer;
		[SerializeField]
		MeshRenderer m_ConvolutionOutputRenderer;
	
		[SerializeField]
		[Range(0, 1)]
		float m_Threshold = 0.69f;

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

		SobelFloatPrototype m_Sobel;

		FloatWithFloatConvolveJob[][] m_ParallelJobSequences = new FloatWithFloatConvolveJob[2][];
	
		void Awake()
		{
			m_Sobel = new SobelFloatPrototype(m_InputTexture);
			SetupTextures();
			//SetupFilter();
			//SetupJobs();

			Debug.Log("awake done");
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
			for (var i = 0; i < m_ParallelJobSequences.Length; i++)
			{
				var sequence = m_ParallelConvolutions.Sequences[i];
				var jobs = new FloatWithFloatConvolveJob[1];
				for (var j = 0; j < jobs.Length; j++)
				{
					jobs[j] = new FloatWithFloatConvolveJob();;
				}
				
				m_ParallelJobSequences[i] = jobs;
				jobs.Cast<IConvolutionJob<float>>().ToArray().AssignSequence(sequence);
			}
		}

		void SetupTextures()
		{
			m_GrayscaleInputTexture = SetupTexture(m_InputTexture, m_GrayscaleRenderer, 
				out m_GrayscaleInputData);
			m_ConvolvedTextureOne = SetupTexture(m_InputTexture, m_GrayscaleRenderer, 
				out m_ConvolvedDataOne);
			m_ConvolvedTextureTwo = SetupTexture(m_InputTexture, m_GrayscaleRenderer, 
				out m_ConvolvedDataTwo);
			m_ConvolutionOutputTexture = SetupTexture(m_InputTexture, m_GrayscaleRenderer, 
				out m_CombinedConvolutionData);
		}

		Texture2D SetupTexture<T>(Texture2D input, Renderer r, out ImageData<T> data)
			where T: struct
		{
			var texture = new Texture2D(m_InputTexture.width, m_InputTexture.height, 
				TextureFormat.RFloat, false);

			data = new ImageData<T>(texture);
			r.material.mainTexture = texture;
			return texture;
		}

		void OnDestroy()
		{
			m_KernelOne.Dispose();
			m_KernelTwo.Dispose();
		}

		void Update ()
		{
			if (Time.frameCount < 10)
				return;
		}
	}
}
