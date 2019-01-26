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
		MeshRenderer m_GaussRenderer;
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
		Texture2D m_GaussTexture;

		ImageData<float> m_GrayscaleInputData;
		ImageData<float> m_GaussInputData;
		
		ImageData<float> m_ConvolvedDataOne;
		ImageData<float> m_ConvolvedDataTwo;
		ImageData<float> m_CombinedConvolutionData;
	
		JobHandle m_GrayScaleJobHandle;
		JobHandle m_JobHandle;

		Kernel<float> m_KernelOne;
		Kernel<float> m_KernelTwo;

		Convolution<float> m_GaussianBlur3x3;
		
		ParallelConvolutions<float> _mParallelConvolutions;

		SobelFloatPrototype m_Sobel;

		GreyscaleByLuminanceFloatJob24 m_GreyscaleJob;
	
		void Awake()
		{
			m_GaussianBlur3x3 = new Convolution<float>(Kernels.Float.GaussianBlurApproximate3x3);
			
			SetupTextures();
		}

		void SetupTextures()
		{
			m_GrayscaleInputTexture = SetupTexture(m_InputTexture, m_GrayscaleRenderer, 
				out m_GrayscaleInputData);

			m_GaussTexture = SetupTexture(m_InputTexture, m_GaussRenderer, out m_GaussInputData);
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
			
			m_Sobel?.Dispose();
		}

		FloatWithFloatConvolveJob m_GaussJob;

		void Update ()
		{
			switch (Time.frameCount)
			{
				case 3:
					m_GreyscaleJob = new GreyscaleByLuminanceFloatJob24(m_InputTexture.GetRawTextureData<Color24>(),
						m_GrayscaleInputData.Buffer, LuminanceWeights.FloatNormalized);
			
					m_GrayScaleJobHandle = m_GreyscaleJob.Schedule(m_GrayscaleInputData.Buffer.Length, 1024);

					var width = m_InputTexture.width;
					var height = m_InputTexture.width;
					m_GaussJob = new FloatWithFloatConvolveJob()
					{
						Convolution = m_GaussianBlur3x3,
						Input = new ImageData<float>(m_GreyscaleJob.Grayscale, width, height),
						Output = m_GaussInputData
					};

					m_GrayScaleJobHandle = m_GaussJob.Schedule(m_GrayScaleJobHandle);
					break;
				case 8:
					m_GrayScaleJobHandle.Complete();
					m_GrayscaleInputTexture.LoadRawTextureData(m_GreyscaleJob.Grayscale);
					m_GrayscaleInputTexture.Apply();
					
					m_GaussTexture.LoadImageData(m_GaussJob.Output);
				
					m_Sobel = new SobelFloatPrototype(m_GaussJob.Output);
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
