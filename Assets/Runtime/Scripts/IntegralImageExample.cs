using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class IntegralImageExample : MonoBehaviour
{
	[SerializeField]
	Texture2D m_Texture;
	
	[SerializeField]
	Texture2D m_GrayScaleTexture;
	
	[SerializeField]
	Texture2D m_GrayScaleTexture8;

	[SerializeField]
	MeshRenderer m_TextureOneRenderer;
	
	[SerializeField]
	MeshRenderer m_TextureTwoRenderer;
	
	[SerializeField]
	[Range(32, 1000f)]
	float m_Threshold = 128f;
	

	NativeArray<Color24> m_InputTextureData;
	
	// Alpha-8 texture with grayscale color encoded in alpha channel
	NativeArray<byte> m_GrayTextureData8;
	NativeArray<byte> m_SobelTextureData8;
	
	NativeArray<float> m_IntegralImageData;
	NativeArray<int> m_IntegralImageDataInt;
	NativeArray<float> m_MeanIntensity3x3;
	
	NativeArray<int> m_MeanIntensity3x3short;


	JobHandle m_IntensityJobHandle;
	JobHandle m_GrayScaleJobHandle;
	JobHandle m_GrayScale8JobHandle;
	JobHandle m_GrayScale24JobHandle;
	JobHandle m_JobHandle;
	JobHandle m_ByteIntegralJobHandle;
	
	
	JobHandle m_EndingByteJobHandle;


	void Start()
	{
		m_InputTextureData = m_Texture.GetRawTextureData<Color24>();
		m_GrayTextureData8 = new NativeArray<byte>(m_InputTextureData.Length, Allocator.Persistent);
		m_SobelTextureData8 = new NativeArray<byte>(m_InputTextureData.Length, Allocator.Persistent);
		
		m_GrayScaleTexture = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.Alpha8, false) 
			{alphaIsTransparency = true};
		m_GrayScaleTexture8 = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.Alpha8, false) 
				{alphaIsTransparency = true};

		m_IntegralImageData = new NativeArray<float>(m_InputTextureData.Length, Allocator.Persistent);
		m_IntegralImageDataInt = new NativeArray<int>(m_InputTextureData.Length, Allocator.Persistent);
		m_MeanIntensity3x3 = new NativeArray<float>(m_InputTextureData.Length, Allocator.Persistent);
		m_MeanIntensity3x3short = new NativeArray<int>(m_InputTextureData.Length, Allocator.Persistent);

		var grayscale8Job = new Grayscale8FromColor24Job()
		{
			InputTexture = m_InputTextureData,
			Grayscale = m_GrayTextureData8,
		};

		var byteIntegralJob = new IntegralImageFromGrayscaleByteJob()
		{
			GrayscaleTexture = m_GrayTextureData8,
			IntegralTexture = m_IntegralImageDataInt,
			width = m_Texture.width,
			height = m_Texture.height
		};

		var meanJob = new AverageIntensity3x3Job()
		{
			Integral = m_IntegralImageData,
			Intensities = m_MeanIntensity3x3,
			height = m_Texture.height,
			width = m_Texture.width
		};
		
		var meanIntJob = new AverageIntensity3x3IntJob()
		{
			Integral = m_IntegralImageDataInt,
			Intensities = m_MeanIntensity3x3short,
			height = m_Texture.height,
			width = m_Texture.width
		};

		m_GrayScale8JobHandle = grayscale8Job.Schedule(m_InputTextureData.Length, 4096);

		m_ByteIntegralJobHandle = byteIntegralJob.Schedule(m_GrayScale8JobHandle);

		m_EndingByteJobHandle = meanIntJob.Schedule(m_ByteIntegralJobHandle);

		m_IntensityJobHandle = meanJob.Schedule(m_JobHandle);
	}

	void OnDestroy()
	{
		if(m_IntegralImageData.IsCreated)
			m_IntegralImageData.Dispose();
		if(m_IntegralImageDataInt.IsCreated)
			m_IntegralImageDataInt.Dispose();
		if(m_MeanIntensity3x3.IsCreated)
			m_MeanIntensity3x3.Dispose();
	}

	bool m_ScheduledLastUpdate;
	
	void Update ()
	{
		if (Time.frameCount < 4)
			return;
		
		if (Time.frameCount == 4)
		{
			m_IntensityJobHandle.Complete();
			m_EndingByteJobHandle.Complete();
		
			m_GrayScaleTexture.LoadRawTextureData(m_GrayTextureData8);
			m_GrayScaleTexture.Apply();
		
			m_GrayScaleTexture8.LoadRawTextureData(m_GrayTextureData8);
			m_GrayScaleTexture8.Apply();

			m_TextureOneRenderer.material.mainTexture = m_GrayScaleTexture;
			m_TextureTwoRenderer.material.mainTexture = m_GrayScaleTexture8;
			return;
		}

		if (Time.frameCount < 60)
			return;

		if (!m_ScheduledLastUpdate)
		{
			var sobelJob = new SobelJob()
			{
				width = m_Texture.width,
				height = m_Texture.height,
				threshold = m_Threshold + Mathf.Sin(Time.time) * 32f,
				Grayscale = m_GrayTextureData8,
				SobelOut = m_SobelTextureData8
			};

			m_EndingByteJobHandle = sobelJob.Schedule(m_GrayScale8JobHandle);
			m_ScheduledLastUpdate = true;
		}
		else
		{
			m_EndingByteJobHandle.Complete();
			m_ScheduledLastUpdate = false;
			
			m_GrayScaleTexture8.LoadRawTextureData(m_SobelTextureData8);
			m_GrayScaleTexture8.Apply();
		}



		
		//Debug.Log("trying sobel...");
		
		//Sobel.Execute(m_GrayTextureData8, m_SobelTextureData8, m_Texture.width, m_Texture.height);
		
		//m_GrayScaleTexture8.LoadRawTextureData(m_SobelTextureData8);
		//m_GrayScaleTexture8.Apply();
	}
}
