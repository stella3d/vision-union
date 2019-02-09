using Unity.Jobs;
using UnityEngine;
using VisionUnion.Jobs;
using VisionUnion.Organization;
using VisionUnion.Visualization.Utils;

namespace VisionUnion.Examples
{
	public class FloatVectorizationExample : MonoBehaviour
	{
		[SerializeField]
		Texture2D m_InputTexture1;
		[SerializeField]
		Texture2D m_InputTexture2;
		[SerializeField]
		Texture2D m_InputTexture3;
		[SerializeField]
		Texture2D m_InputTexture4;
	
		[SerializeField]
		MeshRenderer m_GrayscaleInputRenderer;
		[SerializeField]
		MeshRenderer m_ConvolutionOutputRenderer;
		[SerializeField]
		MeshRenderer m_ActivatedOutputRenderer;
	
		Texture2D m_ConvolvedTexture;
		Texture2D m_ActivatedConvolvedTexture;

		ImageData<float> m_GrayscaleInputData;
		ImageData<float> m_ConvolvedData;
		ImageData<float> m_ActivatedData;

		Convolution2D<float> m_Convolution;

		JobHandle m_JobHandle;

		GreyscaleByLuminanceFloatJob24 m_GreyscaleJob;
		FloatWithFloatConvolveJob m_ConvolveJob;
		BiasedReluActivationCopyJob m_BiasedReluJob;
	
		void Awake()
		{
			m_Convolution = new Convolution2D<float>(Kernels.Short.Sobel.X.ToFloat());
			
			SetupTextures();

			var inputData = new ImageData<Color24>(m_InputTexture);
			m_GreyscaleJob = new GreyscaleByLuminanceFloatJob24(inputData.Buffer, 
				m_GrayscaleInputData.Buffer, LuminanceWeights.FloatNormalized);
			
			m_ConvolveJob = new FloatWithFloatConvolveJob(m_Convolution, m_GrayscaleInputData, m_ConvolvedData);
			m_BiasedReluJob = new BiasedReluActivationCopyJob(m_ConvolvedData.Buffer, m_ActivatedData.Buffer, 0f);

			m_JobHandle = m_GreyscaleJob.Schedule(m_GrayscaleInputData.Buffer.Length, 4096);
			m_JobHandle = m_ConvolveJob.Schedule(m_JobHandle);
			m_JobHandle = m_BiasedReluJob.Schedule(m_ConvolvedData.Buffer.Length, 4096, m_JobHandle);
		}

		void SetupTextures()
		{
			var input = m_InputTexture;
			m_GrayscaleInputData = new ImageData<float>(input.width, input.height);
			
			m_ConvolvedTexture = TextureUtils.SetupImage(input.width, input.height, 
				out m_ConvolvedData, TextureFormat.RFloat);

			m_ConvolutionOutputRenderer.material.mainTexture = m_ConvolvedTexture;
			
			m_ActivatedConvolvedTexture = TextureUtils.SetupImage(input.width, input.height, 
				out m_ActivatedData, TextureFormat.RFloat);
			
			m_ActivatedOutputRenderer.material.mainTexture = m_ActivatedConvolvedTexture;
		}

		void OnDestroy()
		{
			m_GrayscaleInputData.Dispose();
			m_Convolution.Dispose();
		}

		void Update ()
		{
			switch (Time.frameCount)
			{
				case 3:
					m_JobHandle.Complete();
					break;
				case 4:
					m_ConvolvedTexture.LoadImageData(m_ConvolvedData);
					m_ActivatedConvolvedTexture.LoadImageData(m_ActivatedData);
					break;
			}
		}
	}
}
