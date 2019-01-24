using System;
using System.Collections;
using System.Collections.Generic;

namespace VisionUnion.Organization
{
    /// <summary>
    /// A set of convolutions that runs serially, each operating on the output of the previous
    /// </summary>
    /// <typeparam name="T">The kernel type of the convolution</typeparam>
    public class SeparatedFloatRgbFilter : IDisposable
    {
        FloatParallelConvolutionJobs m_Red;
        FloatParallelConvolutionJobs m_Green;
        FloatParallelConvolutionJobs m_Blue;

        public SeparatedFloatRgbFilter()
        {
        }

        public void Dispose()
        {
            
        }
    }
}