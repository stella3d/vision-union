using System;
using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
    /// <summary>
    /// RGB ???
    /// </summary>
    public class SeparatedFloatRgbFilter : IDisposable 
    {
        FloatParallelConvolutionJobs m_Red;
        FloatParallelConvolutionJobs m_Green;
        FloatParallelConvolutionJobs m_Blue;

        public SeparatedFloatRgbFilter(ImageData<float>[] channels, 
            ParallelConvolutionData<float>[] data)
        {
            m_Red = new FloatParallelConvolutionJobs(channels[0], data[0]);
            m_Green = new FloatParallelConvolutionJobs(channels[1], data[1]);
            m_Blue = new FloatParallelConvolutionJobs(channels[2], data[2]);
        }

        public void Dispose()
        {
            m_Red.Dispose();
            m_Green.Dispose();
            m_Blue.Dispose();
        }
    }
}