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

		GreyscaleByLuminanceFloatJob24 m_GreyscaleJob;
	
		void Awake()
		{
			SetupTextures();

		}

		void SetupTextures()
		{
			m_GrayscaleInputTexture = SetupTexture(m_InputTexture, m_GrayscaleRenderer, 
				out m_GrayscaleInputData);
			m_ConvolvedTextureOne = SetupTexture(m_GrayscaleInputTexture, m_GrayscaleRenderer, 
				out m_ConvolvedDataOne);
			m_ConvolvedTextureTwo = SetupTexture(m_GrayscaleInputTexture, m_GrayscaleRenderer, 
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
			if (Time.frameCount == 2)
			{
				m_GreyscaleJob = new GreyscaleByLuminanceFloatJob24(m_InputTexture.GetRawTextureData<Color24>(),
					m_GrayscaleInputData.Buffer, LuminanceWeights.Float);
			
				m_GrayScaleJobHandle = m_GreyscaleJob.Schedule(m_GrayscaleInputData.Buffer.Length, 1024);
				Debug.Log("scheduled grayscale ?");
			}
			
			if (Time.frameCount == 6)
			{
				m_GrayScaleJobHandle.Complete();
				m_GrayscaleInputTexture.LoadRawTextureData(m_GreyscaleJob.Grayscale);
				m_GrayscaleInputTexture.Apply();
				Debug.Log("completed grayscale ?");
				
				m_Sobel = new SobelFloatPrototype(m_GrayscaleInputTexture);
			}
			
			if (Time.frameCount == 7)
			{

			}

			if (Time.frameCount == 10)
			{
				Debug.Log("awake done");
			}
		}
	}
}
