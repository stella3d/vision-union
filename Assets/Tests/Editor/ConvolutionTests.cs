using System.Linq;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;

namespace VisionUnion.Tests
{
	public class ConvolutionTests
	{
		Texture2D m_GrayScaleTexture8;
		Texture2D m_IntermediateTexture16;
		
		Texture2D m_FloatInputTexture;
		Texture2D m_FloatOutputTexture;

		NativeArray<Color24> m_InputTextureData;

		ImageData<byte> m_InputImage;
		ImageData<short> m_IntermediateImage;
		
		ImageData<float> m_InputFloatImage;
		ImageData<float> m_IntermediateFloatImage;

		const byte inputColorOne = 42;

		[OneTimeSetUp]
		public void BeforeAll()
		{
			m_GrayScaleTexture8 = DebugUtils.NewFilledTexture(8, 8, inputColorOne, TextureFormat.R8);
			m_InputImage = new ImageData<byte>(m_GrayScaleTexture8);
			
			m_IntermediateTexture16 = new Texture2D(8, 8, TextureFormat.R16, false);
			m_IntermediateImage = new ImageData<short>(m_IntermediateTexture16);
			
			m_FloatInputTexture = DebugUtils.NewFilledTexture(8, 8, 0.5f, TextureFormat.RFloat);
			m_InputFloatImage = new ImageData<float>(m_FloatInputTexture);
			m_FloatOutputTexture = new Texture2D(8, 8, TextureFormat.RFloat, false);
			m_IntermediateFloatImage = new ImageData<float>(m_FloatInputTexture);
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
			var kernel = new Kernel<short>(Kernels.Short.Identity1x1);
			var convolution = new Convolution<short>(kernel, 1, 0);
			
			convolution.Convolve(m_InputImage, m_IntermediateImage);
			m_InputImage.Buffer.AssertDeepEqual(m_IntermediateImage.Buffer);
			convolution.Dispose();
		}
		
		[Test]
		public void ConvolutionWithIdentityKernel_OutputEqualsInput_3x3()
		{
			var kernel = new Kernel<short>(Kernels.Short.Identity3x3);
			var convolution = new Convolution<short>(kernel, 1, 1);
			
			convolution.Convolve(m_InputImage, m_IntermediateImage);
			// TODO - iterate through, check padding ?
			convolution.Dispose();
		}
		
		[TestCaseSource(typeof(ExpectedConvolutionResults), "FloatCases")]
		public void ConvolutionWith_1x1Stride_1x1Pad(int width, int height, 
			float[,] kernelInput, float[] input, float[] expectedInput)
		{
			var kernel = new Kernel<float>(kernelInput, Allocator.Temp);
			var convolution = new Convolution<float>(kernel, 1, 1);
			
			var image = new ImageData<float>(input, width, height, Allocator.Temp);
			var expected = new ImageData<float>(expectedInput, width, height, Allocator.Temp);
			var output = new ImageData<float>(new float[width * height], width, height, Allocator.Temp);

			convolution.Convolve(image, output);
			output.Print();
			
			expected.AssertEqualWithin(output);

			convolution.Dispose();
			image.Dispose();
			output.Dispose();
			expected.Dispose();
		}
	}
}