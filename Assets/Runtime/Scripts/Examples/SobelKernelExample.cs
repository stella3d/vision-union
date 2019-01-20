using System.Collections;
using System.Collections.Generic;
using VisionUnion;
using VisionUnion.Jobs;
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
	
	Kernel<short> m_KernelOutline;
	
	Kernel<float> m_KernelBoxBlur;
	Kernel<float> m_KernelGaussianBlur;
	
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
		
		m_KernelX = new Kernel<short>(Kernels.Short.Sobel.X);
		m_KernelY = new Kernel<short>(Kernels.Short.Sobel.Y);
		
		m_KernelOutline = new Kernel<short>(Kernels.Short.Outline);
		
		m_KernelBoxBlur = new Kernel<float>(Kernels.Float.BoxBlur);
		m_KernelGaussianBlur = new Kernel<float>(Kernels.Float.GaussianBlurApproximate5x5);
		/*

		var grayscale8Job = new Grayscale8FromColor24Job()
		{
			InputTexture = m_InputTextureData,
			Grayscale = m_GrayTextureData8,
		};
		
		var kernelOne = new Kernel<short>(Kernels.Short.Sobel.X);
		var sharpenJob = new ShortKernelConvolveJob(kernelOne, m_GrayTextureData8, m_SobelTextureDataX,
			m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
		
		var kernelTwo = new Kernel<short>(Kernels.Short.Sobel.Y);
		var sharpenJob2 = new ShortKernelConvolveJob(kernelTwo, m_GrayTextureData8, m_SobelTextureDataY,
			m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
		
		m_GrayScaleJobHandle = grayscale8Job.Schedule(m_InputTextureData.Length, 4096);
		
		m_JobHandle = sharpenJob.Schedule(m_GrayScaleJobHandle);
		m_JobHandle = sharpenJob2.Schedule(m_JobHandle);
		*/

		//var gauss5x5Kernel = new Kernel<float>(Kernels.GaussianBlurApproximate5x5);
		//var gaussJob = new FloatKernelConvolveJob(gauss5x5Kernel, m_GrayTextureData8, m_SobelTextureDataY,
		//	m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);

		//m_JobHandle = gaussJob.Schedule(m_JobHandle);
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
			m_JobHandle.Complete();
					
			m_GrayScaleTexture8.LoadRawTextureData(m_GrayTextureData8);
			m_GrayScaleTexture8.Apply();
			//KernelOperations.Run(m_GrayTextureData8, m_SobelTextureDataX, m_KernelX,
			//	m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
			
			//m_KernelOutline.Convolve(m_GrayTextureData8, m_SobelTextureDataX,
			//	m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);

			m_SobelTextureX.LoadRawTextureData(m_SobelTextureDataX);
			m_SobelTextureX.Apply();
			return;
		}
		
		if (Time.frameCount == 18)
		{
			//KernelOperations.Run(m_GrayTextureData8, m_SobelTextureDataY, m_KernelY,
			//	m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
			
			//m_KernelGaussianBlur.Convolve(m_GrayTextureData8, m_SobelTextureDataY,
			//	m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
			
			m_SobelTextureY.LoadRawTextureData(m_SobelTextureDataY);
			m_SobelTextureY.Apply();
			return;
		}
		
		if (!m_ScheduledLastUpdate)
		{
			//m_KernelBoxBlur.Convolve(m_GrayTextureData8, m_SobelTextureDataCombined,
			//	m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
			
			m_JobHandle.Complete();
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
