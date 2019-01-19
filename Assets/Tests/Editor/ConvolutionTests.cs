using System;
using System.Security.Cryptography.X509Certificates;
using VisionUnion;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;

namespace VisionUnion.Tests
{
	public class ConvolutionTests
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
			const int count = 16 * 16;
			m_GrayScaleTexture8 = new Texture2D(16, 16);
			m_GrayTextureData8 = new NativeArray<byte>(count, Allocator.Temp);
			m_SobelTextureDataX = new NativeArray<float>(count, Allocator.Temp);
			m_SobelTextureDataCombined = new NativeArray<float>(count, Allocator.Temp);
		}

		[OneTimeTearDown]
		public void AfterAll()
		{
			m_GrayTextureData8.Dispose();
			m_SobelTextureDataX.Dispose();
			m_SobelTextureDataCombined.Dispose();
		}

		// while we would never actually do this convolution, it is useful 
		// to test that convolution itself is working correctly
		[Test]
		public void IdentityKernelConvolution_OutputEqualsInput()
		{
			var identityKernel = new Kernel<byte>(Kernels.Byte.Identity);
			
			identityKernel.Dispose();
		}
	}
}