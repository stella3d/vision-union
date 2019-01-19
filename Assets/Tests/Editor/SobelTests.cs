using System.Collections;
using System.Collections.Generic;
using System.Text;
using VisionUnion;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class SobelTests
{
	Texture2D m_GrayScaleTexture8;
	
	NativeArray<Color24> m_InputTextureData;
	
	// Alpha-8 texture with grayscale color encoded in alpha channel
	NativeArray<byte> m_GrayTextureData8;
	
	NativeArray<float> m_SobelTextureDataX;
	NativeArray<float> m_SobelTextureDataY;
	NativeArray<float> m_SobelTextureDataCombined;
	
	[Test]
	public void SeparatedResultIsTheSame()
	{
		var kX = new Kernel<short>(Kernels.Sobel.xHorizontal);
		var kY = new Kernel<short>(Kernels.Sobel.xVertical);
			
		KernelOperations.RunHorizontal1D(m_GrayTextureData8, m_SobelTextureDataX, kX,
			m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);

		KernelOperations.RunVertical1D(m_SobelTextureDataX, m_SobelTextureDataX, kY,
			m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
	}
}
