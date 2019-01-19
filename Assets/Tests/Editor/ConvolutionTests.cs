using System;
using VisionUnion;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;

namespace VisionUnion.Tests
{
	public class ConvolutionTests
	{
		Texture2D m_GrayScaleTexture8;
		Texture2D m_IntermediateTexture16;

		NativeArray<Color24> m_InputTextureData;

		// Alpha-8 texture with grayscale color encoded in alpha channel
		NativeArray<byte> m_GrayTextureData8;


		NativeArray<float> m_SobelTextureDataX;
		NativeArray<float> m_SobelTextureDataCombined;

		ImageData<byte> m_InputImage;

		NativeArray<short> m_IntermediateData;
		ImageData<short> m_IntermediateImage;

		[OneTimeSetUp]
		public void BeforeAll()
		{
			const int count = 16 * 16;
			
			m_GrayScaleTexture8 = new Texture2D(16, 16);
			m_InputImage = new ImageData<byte>(m_GrayScaleTexture8);
			
			m_IntermediateTexture16 = new Texture2D(16, 16, TextureFormat.R16, false);
			m_IntermediateImage = new ImageData<short>(m_IntermediateTexture16);
			
			m_GrayTextureData8 = new NativeArray<byte>(count, Allocator.Temp);
			m_IntermediateData = new NativeArray<short>(count, Allocator.Temp);
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
		public void ConvolutionWithIdentityKernel_OutputEqualsInput()
		{
			var kernel = new Kernel<short>(Kernels.Short.Identity);
			var convolution = new Convolution<short>(kernel, 1, 0);
			
			convolution.Convolve(m_InputImage, m_IntermediateImage);
			
			//m_InputImage.Buffer.AssertDeepEqual(m_IntermediateImage.Buffer);
			
			
			convolution.Dispose();
		}
	}
}