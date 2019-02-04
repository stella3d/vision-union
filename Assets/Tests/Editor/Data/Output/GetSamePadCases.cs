using System.Collections;
using NUnit.Framework;

namespace VisionUnion.Tests
{
    internal static class GetSamePadCases
    {
        public static IEnumerable Uniform
        {
            get
            {
                var inputImageData = new ImageData<byte>(InputImages.Byte5x5, 5, 5);
                
                var outlineKernel = new Kernel2D<short>(Kernels.Short.Outline);
                var outLineConvolution = new Convolution<short>(outlineKernel);
                yield return new TestCaseData(inputImageData, outLineConvolution, new Padding(1));
                
                var gauss5x5Kernel = new Kernel2D<float>(Kernels.Float.GaussianBlurApproximate5x5);
                var gauss5x5Convolution = new Convolution<float>(gauss5x5Kernel);
                yield return new TestCaseData(inputImageData, gauss5x5Convolution, new Padding(2));
            }
        }
    }
}