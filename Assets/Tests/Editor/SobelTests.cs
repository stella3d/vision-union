using VisionUnion;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;

namespace VisionUnion.Tests
{
	public class SobelTests
	{
		Texture2D m_GrayScaleTexture8;

		NativeArray<Color24> m_InputTextureData;

		// Alpha-8 texture with grayscale color encoded in alpha channel
		NativeArray<byte> m_GrayTextureData8;

		NativeArray<float> m_SobelTextureDataX;
		NativeArray<float> m_SobelTextureDataCombined;

		[OneTimeSetUp]
		public void BeforeAll()
		{
			m_GrayScaleTexture8 = new Texture2D(64, 64);
			m_GrayTextureData8 = new NativeArray<byte>();
			m_SobelTextureDataX = new NativeArray<float>();
			m_SobelTextureDataCombined = new NativeArray<float>();
		}

		[OneTimeTearDown]
		public void AfterAll()
		{
			m_GrayTextureData8.Dispose();
			m_SobelTextureDataX.Dispose();
			m_SobelTextureDataCombined.Dispose();
		}

		[Test]
		public void SeparatedResultIsTheSame()
		{
			var kX = new Kernel<short>(Kernels.Sobel.xHorizontal);
			var kY = new Kernel<short>(Kernels.Sobel.xVertical);

			KernelOperations.RunHorizontal1D(m_GrayTextureData8, m_SobelTextureDataX, kX,
				m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);

			KernelOperations.RunVertical1D(m_SobelTextureDataX, m_SobelTextureDataCombined, kY,
				m_GrayScaleTexture8.width, m_GrayScaleTexture8.height);
		}
	}
}