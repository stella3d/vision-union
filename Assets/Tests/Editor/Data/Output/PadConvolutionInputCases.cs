using System.Collections;
using NUnit.Framework;

namespace VisionUnion.Tests
{
    internal static class PadConvolutionInputCases
    {
        public static IEnumerable Uniform
        {
            get
            {
                var inputImageData = new Image<byte>(InputImages.Byte5x5, 5, 5);
                
                var outlineKernel = new Kernel2D<short>(Kernels.Short.Outline);
                var outLineConvolution = new Convolution2D<short>(outlineKernel);
                var expected1 = new Image<byte>(OutputPadImages.Input5x5ZeroPadUniform1, 7, 7);
                yield return new TestCaseData(inputImageData, outLineConvolution, expected1);
                
                var sobel5x5Kernel = new Kernel2D<short>(Kernels.Short.Sobel.x5x5);
                var sobel5x5Convolution = new Convolution2D<short>(sobel5x5Kernel);
                var expected2 = new Image<byte>(OutputPadImages.Input5x5ZeroPadUniform2, 9, 9);
                yield return new TestCaseData(inputImageData, sobel5x5Convolution, expected2);
            }
        }
    }
}