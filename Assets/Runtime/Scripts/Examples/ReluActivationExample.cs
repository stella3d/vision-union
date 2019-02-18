using Unity.Jobs;
using UnityEngine;
using VisionUnion.Jobs;
using VisionUnion.Organization;
using VisionUnion.Visualization.Utils;

namespace VisionUnion.Examples
{
	public class ReluActivationExample : MonoBehaviour
	{
		[SerializeField]
		Texture2D m_InputTexture;
	
		[SerializeField]
		MeshRenderer m_GrayscaleInputRenderer;
		[SerializeField]
		MeshRenderer m_ConvolutionOutputRenderer;
		[SerializeField]
		MeshRenderer m_ActivatedOutputRenderer;
	
		Texture2D m_ConvolvedTexture;
		Texture2D m_ActivatedConvolvedTexture;

		Image<float> m_GrayscaleInput;
		Image<float> m_Convolved;
		Image<float> m_Activated;

		Convolution2D<float> m_Convolution;

		JobHandle m_JobHandle;

		GreyscaleByLuminanceFloatJob24 m_GreyscaleJob;
		FloatWithFloatConvolveJob m_ConvolveJob;
		BiasedReluActivationCopyJob m_BiasedReluJob;
	
		void Awake()
		{
			m_Convolution = new Convolution2D<float>(Kernels.Short.Sobel.X.ToFloat());
			
			SetupTextures();

			var inputData = new Image<Color24>(m_InputTexture);
			m_GreyscaleJob = new GreyscaleByLuminanceFloatJob24(inputData.Buffer, 
				m_GrayscaleInput.Buffer, LuminanceWeights.FloatNormalized);
			
			m_ConvolveJob = new FloatWithFloatConvolveJob(m_Convolution, m_GrayscaleInput, m_Convolved);
			m_BiasedReluJob = new BiasedReluActivationCopyJob(m_Convolved.Buffer, m_Activated.Buffer, 0f);

			m_JobHandle = m_GreyscaleJob.Schedule(m_GrayscaleInput.Buffer.Length, 4096);
			m_JobHandle = m_ConvolveJob.Schedule(m_JobHandle);
			m_JobHandle = m_BiasedReluJob.Schedule(m_Convolved.Buffer.Length, 4096, m_JobHandle);
		}

		void SetupTextures()
		{
			var input = m_InputTexture;
			m_GrayscaleInput = new Image<float>(input.width, input.height);
			
			m_ConvolvedTexture = TextureUtils.SetupImage(input.width, input.height, 
				out m_Convolved, TextureFormat.RFloat);

			m_ConvolutionOutputRenderer.material.mainTexture = m_ConvolvedTexture;
			
			m_ActivatedConvolvedTexture = TextureUtils.SetupImage(input.width, input.height, 
				out m_Activated, TextureFormat.RFloat);
			
			m_ActivatedOutputRenderer.material.mainTexture = m_ActivatedConvolvedTexture;
		}

		void OnDestroy()
		{
			m_GrayscaleInput.Dispose();
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
					m_ConvolvedTexture.LoadImageData(m_Convolved);
					m_ActivatedConvolvedTexture.LoadImageData(m_Activated);
					break;
			}
		}
	}
}
