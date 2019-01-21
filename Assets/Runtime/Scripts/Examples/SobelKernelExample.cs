using Unity.Jobs;
using UnityEngine;

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
	
		void Awake()
		{
			m_GrayscaleInputTexture = new Texture2D(m_InputTexture.width, m_InputTexture.height, 
				TextureFormat.RFloat, false);
			m_ConvolvedTextureOne = new Texture2D(m_InputTexture.width, m_InputTexture.height, 
				TextureFormat.RFloat, false);
			m_ConvolvedTextureTwo = new Texture2D(m_InputTexture.width, m_InputTexture.height, 
				TextureFormat.RFloat, false);
			m_ConvolutionOutputTexture = new Texture2D(m_InputTexture.width, m_InputTexture.height, 
				TextureFormat.RFloat, false);
		
			m_GrayscaleInputData = new ImageData<float>(m_GrayscaleInputTexture);
			m_ConvolvedDataOne = new ImageData<float>(m_ConvolvedTextureOne);
			m_ConvolvedDataTwo = new ImageData<float>(m_ConvolvedTextureTwo);
			m_CombinedConvolutionData = new ImageData<float>(m_ConvolutionOutputTexture);
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
