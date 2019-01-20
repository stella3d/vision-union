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

		ImageData<byte> m_InputImage;

		ImageData<short> m_IntermediateImage;

		const byte inputColorOne = 42;

		[OneTimeSetUp]
		public void BeforeAll()
		{
			m_GrayScaleTexture8 = DebugUtils.NewFilledTexture(8, 8, inputColorOne, TextureFormat.R8);
			m_InputImage = new ImageData<byte>(m_GrayScaleTexture8);
			
			m_IntermediateTexture16 = new Texture2D(8, 8, TextureFormat.R16, false);
			m_IntermediateImage = new ImageData<short>(m_IntermediateTexture16);
		}

		[OneTimeTearDown]
		public void AfterAll()
		{
		}

		// while we would never actually do this convolution, it is useful 
		// to test that convolution itself is working correctly
		[Test]
		public void ConvolutionWithIdentityKernel_OutputEqualsInput_1x1()
		{
			var kernel = new Kernel<byte>(Kernels.Byte.Identity1x1);
			var convolution = new Convolution<byte>(kernel, 1, 0);
			
			convolution.Convolve(m_InputImage, m_IntermediateImage);
			m_InputImage.Buffer.AssertDeepEqual(m_IntermediateImage.Buffer);
			convolution.Dispose();
		}
		
		[Test]
		public void ConvolutionWithIdentityKernel_OutputEqualsInput_3x3()
		{
			var kernel = new Kernel<byte>(Kernels.Byte.Identity3x3);
			var convolution = new Convolution<byte>(kernel, 1, 1);
			
			convolution.Convolve(m_InputImage, m_IntermediateImage);
			//m_InputImage.Buffer.AssertDeepEqual(m_IntermediateImage.Buffer);
			convolution.Dispose();
		}
	}
}