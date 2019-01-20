using System.Collections;
using NUnit.Framework;

namespace VisionUnion.Tests
{
    public static class ExpectedConvolutionResults
    {
        public static IEnumerable FloatCases
        {
            get
            {
                yield return new TestCaseData(5, 5, Kernels.Float.BoxBlur, 
                    InputImages.Float5x5, OutputImages.Post3x3BoxBlur5x5);
                yield return new TestCaseData(5, 5, Kernels.Float.GaussianBlurApproximate3x3, 
                    InputImages.Float5x5, OutputImages.Post3x3GaussianBlur5x5);
            }
        }
    }
}