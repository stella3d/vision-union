using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class SobelKernelExample : MonoBehaviour
{
	[SerializeField]
	Texture2D m_Texture;
	
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
	[Range(0, 255)]
	short m_Threshold = 128;
	
	NativeArray<Color24> m_InputTextureData;
	
	// Alpha-8 texture with grayscale color encoded in alpha channel
	NativeArray<byte> m_GrayTextureData8;
	
	NativeArray<short> m_SobelTextureDataX;
	NativeArray<short> m_SobelTextureDataY;

	JobHandle m_GrayScaleJobHandle;
	JobHandle m_JobHandle;
	

	void Start()
	{
		m_InputTextureData = m_Texture.GetRawTextureData<Color24>();
		m_GrayTextureData8 = new NativeArray<byte>(m_InputTextureData.Length, Allocator.Persistent);
		
		m_GrayScaleTexture8 = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.Alpha8, false) 
			{alphaIsTransparency = true};

		m_TextureOneRenderer.material.mainTexture = m_GrayScaleTexture8;
		
		m_SobelTextureDataX = new NativeArray<short>(m_InputTextureData.Length, Allocator.Persistent);
		m_SobelTextureDataY = new NativeArray<short>(m_InputTextureData.Length, Allocator.Persistent);
		
		m_SobelTextureX = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.R16, false) 
			{alphaIsTransparency = true};
		
		m_XKernelRenderer.material.mainTexture = m_SobelTextureX;
		
		m_SobelTextureY = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.R16, false) 
			{alphaIsTransparency = true};
		
		m_YKernelRenderer.material.mainTexture = m_SobelTextureY;

		var grayscale8Job = new Grayscale8FromColor24Job()
		{
			InputTexture = m_InputTextureData,
			Grayscale = m_GrayTextureData8,
		};
		
		m_GrayScaleJobHandle = grayscale8Job.Schedule(m_InputTextureData.Length, 4096);
	}

	void OnDestroy()
	{
		if(m_GrayTextureData8.IsCreated)
			m_GrayTextureData8.Dispose();
	}

	bool m_ScheduledLastUpdate;
	
	void Update ()
	{
		if (Time.frameCount < 20)
			return;
		
		if (!m_ScheduledLastUpdate)
		{
			m_GrayScaleJobHandle.Complete();
			m_GrayScaleTexture8.LoadRawTextureData(m_GrayTextureData8);
			m_GrayScaleTexture8.Apply();
			m_ScheduledLastUpdate = true;
		}
		else
		{
			m_ScheduledLastUpdate = false;
			
			m_SobelTextureX.LoadRawTextureData(m_SobelTextureDataX);
			m_SobelTextureX.Apply();
			m_SobelTextureY.LoadRawTextureData(m_SobelTextureDataY);
			m_SobelTextureY.Apply();
		}
	}
}
