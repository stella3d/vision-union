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

		Image<float> m_GrayscaleInput;
		Image<float> m_GaussInput;
		
		Image<float> m_ConvolvedOne;
		Image<float> m_ConvolvedTwo;
		Image<float> m_CombinedConvolution;
	
		JobHandle m_GrayScaleJobHandle;
		JobHandle m_JobHandle;

		Convolution2D<float> m_GaussianBlur3x3;
		
		SobelFloatPrototype m_Sobel;

		GreyscaleByLuminanceFloatJob24 m_GreyscaleJob;

		BiasedReluActivationJob _mBiasedReluJob;
	
		void Awake()
		{
			m_GaussianBlur3x3 = new Convolution2D<float>(Kernels.Float.GaussianBlurApproximate3x3);
			
			SetupTextures();
		}

		void SetupTextures()
		{
			m_GrayscaleInputTexture = SetupTexture(m_InputTexture, m_GrayscaleRenderer, 
				out m_GrayscaleInput);

			m_GaussTexture = SetupTexture(m_InputTexture, m_GaussRenderer, out m_GaussInput);
		}

		Texture2D SetupTexture<T>(Texture2D input, Renderer r, out Image<T> data)
			where T: struct
		{
			var texture = new Texture2D(input.width, input.height, 
				TextureFormat.RFloat, false);

			data = new Image<T>(texture);
			r.material.mainTexture = texture;
			return texture;
		}

		void OnDestroy()
		{
			m_GaussianBlur3x3.Dispose();
			m_Sobel?.Dispose();
		}

		FloatWithFloatConvolveJob m_GaussJob;

		void Update ()
		{
			switch (Time.frameCount)
			{
				case 3:
					m_GreyscaleJob = new GreyscaleByLuminanceFloatJob24(m_InputTexture.GetRawTextureData<Color24>(),
						m_GrayscaleInput.Buffer, LuminanceWeights.FloatNormalized);
			
					m_GrayScaleJobHandle = m_GreyscaleJob.Schedule(m_GrayscaleInput.Buffer.Length, 1024);

					var width = m_InputTexture.width;
					var height = m_InputTexture.width;
					m_GaussJob = new FloatWithFloatConvolveJob()
					{
						Convolution = m_GaussianBlur3x3,
						Input = new Image<float>(m_GreyscaleJob.Grayscale, width, height),
						Output = m_GaussInput
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
				case 24:
					m_KernelOneRenderer.material.mainTexture = m_Sobel.ConvolvedTextureOne;
					m_KernelTwoRenderer.material.mainTexture = m_Sobel.ConvolvedTextureTwo;
					m_ConvolutionOutputRenderer.material.mainTexture = m_Sobel.ConvolutionOutputTexture;
					break;
			}
			
		}
	}
}
