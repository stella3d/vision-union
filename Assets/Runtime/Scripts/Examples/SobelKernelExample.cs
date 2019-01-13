using System.Collections;
using System.Collections.Generic;
using BurstVision;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class SobelKernelExample : MonoBehaviour
{
	[SerializeField]
	Texture2D m_Texture;
	
	Texture2D m_SobelTexture;
	Texture2D m_SobelTextureX;
	Texture2D m_SobelTextureY;
	
	Texture2D m_GrayScaleTexture8;

	[SerializeField]
	MeshRenderer m_TextureOneRenderer;
	
	[SerializeField]
	MeshRenderer m_XKernelRenderer;
	
	[SerializeField]
	MeshRenderer m_YKernelRenderer;
	
	[SerializeField]
	MeshRenderer m_SobelOutputRenderer;
	
	[SerializeField]
	[Range(0, 1)]
	float m_Threshold = 0.69f;
	
	NativeArray<Color24> m_InputTextureData;
	
	// Alpha-8 texture with grayscale color encoded in alpha channel
	NativeArray<byte> m_GrayTextureData8;
	
	NativeArray<float> m_SobelTextureDataX;
	NativeArray<float> m_SobelTextureDataY;
	NativeArray<float> m_SobelTextureDataCombined;

	JobHandle m_GrayScaleJobHandle;
	JobHandle m_JobHandle;

	Kernel<short> m_KernelX;
	Kernel<short> m_KernelY;
	

	void Start()
	{
		m_InputTextureData = m_Texture.GetRawTextureData<Color24>();
		m_GrayTextureData8 = new NativeArray<byte>(m_InputTextureData.Length, Allocator.Persistent);
		
		m_GrayScaleTexture8 = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.Alpha8, false) 
			{alphaIsTransparency = true};

		m_TextureOneRenderer.material.mainTexture = m_GrayScaleTexture8;
		
		m_SobelTextureDataX = new NativeArray<float>(m_InputTextureData.Length, Allocator.Persistent);
		m_SobelTextureDataY = new NativeArray<float>(m_InputTextureData.Length, Allocator.Persistent);
		m_SobelTextureDataCombined = new NativeArray<float>(m_InputTextureData.Length, Allocator.Persistent);
		
		m_SobelTextureX = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.RFloat, false) 
			{alphaIsTransparency = true};
		
		m_XKernelRenderer.material.mainTexture = m_SobelTextureX;
		
		m_SobelTextureY = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.RFloat, false) 
			{alphaIsTransparency = true};
		
		m_YKernelRenderer.material.mainTexture = m_SobelTextureY;
		
		m_SobelTexture = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.RFloat, false) 
			{alphaIsTransparency = true};
		
		m_SobelOutputRenderer.material.mainTexture = m_SobelTexture;
		
		m_KernelX = new Kernel<short>(Kernels.Sobel.X);
		m_KernelY = new Kernel<short>(Kernels.Sobel.Y);

		var grayscale8Job = new Grayscale8FromColor24Job()
		{
			InputTexture = m_InputTextureData,
			Grayscale = m_GrayTextureData8,
		};
		
		m_GrayScaleJobHandle = grayscale8Job.Schedule(m_InputTextureData.Length, 4096);
		m_GrayScaleJobHandle.Complete();
		m_GrayScaleTexture8.LoadRawTextureData(m_GrayTextureData8);
		m_GrayScaleTexture8.Apply();
	}

	void OnDestroy()
	{
		m_KernelX.Dispose();
		m_KernelY.Dispose();
		m_SobelTextureDataCombined.Dispose();
		if(m_GrayTextureData8.IsCreated)
			m_GrayTextureData8.Dispose();
	}

	bool m_ScheduledLastUpdate;
	
	void Update ()
	{
		if (Time.frameCount < 10)
			return;
		

		if (Time.frameCount == 15)
		{
			KernelOperations.Run(m_GrayTextureData8, m_SobelTextureDataX, m_KernelX,
				m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
			
			/*
			var kX = new Kernel<short>(Kernels.Sobel.xHorizontal);
			var kY = new Kernel<short>(Kernels.Sobel.xVertical);
			
			KernelOperations.RunHorizontal1D(m_GrayTextureData8, m_SobelTextureDataX, kX,
				m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);

			KernelOperations.RunVertical1D(m_SobelTextureDataX, m_SobelTextureDataX, kY,
				m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
			*/
			
			m_SobelTextureX.LoadRawTextureData(m_SobelTextureDataX);
			m_SobelTextureX.Apply();
			return;
		}
		
		if (Time.frameCount == 18)
		{
			KernelOperations.Run(m_GrayTextureData8, m_SobelTextureDataY, m_KernelY,
				m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
			
			/*
			var kX = new Kernel<short>(Kernels.Sobel.yHorizontal);
			var kY = new Kernel<short>(Kernels.Sobel.yVertical);
			
			KernelOperations.RunHorizontal1D(m_GrayTextureData8, m_SobelTextureDataY, kX,
				m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);

			KernelOperations.RunVertical1D(m_SobelTextureDataY, m_SobelTextureDataY, kY,
				m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
			*/
			
			m_SobelTextureY.LoadRawTextureData(m_SobelTextureDataY);
			m_SobelTextureY.Apply();
			return;
		}
		
		if (!m_ScheduledLastUpdate)
		{
			m_ScheduledLastUpdate = true;
			Operations.SobelCombine(m_SobelTextureDataX, m_SobelTextureDataY, m_SobelTextureDataCombined, m_Threshold);
			m_SobelTexture.LoadRawTextureData(m_SobelTextureDataCombined);
			m_SobelTexture.Apply();
		}
		else
		{
			m_ScheduledLastUpdate = false;
		}
	}
}
