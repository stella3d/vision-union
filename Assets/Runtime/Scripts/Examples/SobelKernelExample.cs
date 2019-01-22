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

		ImageData<float> m_GrayscaleInputData;
		ImageData<float> m_ConvolvedDataOne;
		ImageData<float> m_ConvolvedDataTwo;
		ImageData<float> m_CombinedConvolutionData;
	
		JobHandle m_GrayScaleJobHandle;
		JobHandle m_JobHandle;

		Kernel<float> m_KernelOne;
		Kernel<float> m_KernelTwo;
		
		ParallelConvolutionSequences<float> m_ParallelConvolutionSequences;

		SobelFloatPrototype m_Sobel;

		GreyscaleByLuminanceFloatJob24 m_GreyscaleJob;
	
		void Awake()
		{
			SetupTextures();
			
			m_Sobel = new SobelFloatPrototype(m_GrayscaleInputTexture);
		}

		void SetupTextures()
		{
			m_GrayscaleInputTexture = SetupTexture(m_InputTexture, m_GrayscaleRenderer, 
				out m_GrayscaleInputData);
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
			//m_KernelOne.Dispose();
			//m_KernelTwo.Dispose();
			m_Sobel.Dispose();
		}

		void Update ()
		{
			switch (Time.frameCount)
			{
				case 3:
					m_Sobel.Dispose();
					m_GreyscaleJob = new GreyscaleByLuminanceFloatJob24(m_InputTexture.GetRawTextureData<Color24>(),
						m_GrayscaleInputData.Buffer, LuminanceWeights.FloatNormalized);
			
					m_GrayScaleJobHandle = m_GreyscaleJob.Schedule(m_GrayscaleInputData.Buffer.Length, 1024);
					break;
				case 8:
					m_GrayScaleJobHandle.Complete();
					m_GrayscaleInputTexture.LoadRawTextureData(m_GreyscaleJob.Grayscale);
					m_GrayscaleInputTexture.Apply();
				
					m_Sobel = new SobelFloatPrototype(m_GrayscaleInputTexture);
					break;
				case 9:
					m_JobHandle = m_Sobel.Schedule(m_GrayScaleJobHandle);
					break;
				case 13:
					m_JobHandle.Complete();
					m_Sobel.OnJobsComplete();
					m_KernelOneRenderer.material.mainTexture = m_Sobel.ConvolvedTextureOne;
					m_KernelTwoRenderer.material.mainTexture = m_Sobel.ConvolvedTextureTwo;
					m_ConvolutionOutputRenderer.material.mainTexture = m_Sobel.ConvolutionOutputTexture;
					break;
				case 15:
					m_JobHandle = m_Sobel.Schedule(m_GrayScaleJobHandle);
					break;
				case 22:
					m_JobHandle.Complete();
					m_Sobel.Complete();
					m_Sobel.OnJobsComplete();
					break;
				case 28:
					m_KernelOneRenderer.material.mainTexture = m_Sobel.ConvolvedTextureOne;
					m_KernelTwoRenderer.material.mainTexture = m_Sobel.ConvolvedTextureTwo;
					m_ConvolutionOutputRenderer.material.mainTexture = m_Sobel.ConvolutionOutputTexture;
					break;
				case 30:
					Debug.Log("awake done");
					break;
			}
			
		}
	}
}
