using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace VisionUnion.Tests
{
    internal static class ConvolutionData
    {
        internal static readonly byte[] Input5x5 =
        {
            9, 9, 6, 2, 1,
            7, 8, 7, 3, 2,
            6, 7, 7, 5, 3,
            5, 6, 6, 6, 4,
            3, 5, 5, 7, 5
        };
        
        internal static readonly float[] FloatInput5x5 =
        {
            9, 9, 6, 2, 1,
            7, 8, 7, 3, 2,
            6, 7, 7, 5, 3,
            5, 6, 6, 6, 4,
            3, 5, 5, 7, 5
        };
        
        internal static readonly float[] PostBoxBlur5x5 =
        {
            0.00f, 0.00f, 0.00f, 0.00f, 0.00f,
            0.00f, 7.33f, 6.00f, 4.00f, 0.00f,
            0.00f, 6.56f, 6.11f, 4.78f, 0.00f,
            0.00f, 5.56f, 6.00f, 5.33f, 0.00f,
            0.00f, 0.00f, 0.00f, 0.00f, 0.00f
        };
        
        internal static readonly float[] PostGaussian3x3Blur5x5 =
        {
            0.00f, 0.00f, 0.00f, 0.00f, 0.00f,
            0.00f, 7.50f, 6.19f, 3.81f, 0.00f,
            0.00f, 6.69f, 6.31f, 4.81f, 0.00f,
            0.00f, 5.69f, 6.00f, 5.50f, 0.00f,
            0.00f, 0.00f, 0.00f, 0.00f, 0.00f
        };

        public static class ExpectedConvolutionResults
        {
            public static IEnumerable FloatCases
            {
                get
                {
                    yield return new TestCaseData(5, 5, Kernels.Float.BoxBlur, 
                        FloatInput5x5, PostBoxBlur5x5);
                    yield return new TestCaseData(5, 5, Kernels.Float.GaussianBlurApproximate3x3, 
                        FloatInput5x5, PostGaussian3x3Blur5x5);
                }
            }
        }
    }
}