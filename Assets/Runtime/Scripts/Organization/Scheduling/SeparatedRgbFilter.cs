using System;
using Unity.Jobs;
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
            var handle = new JobHandle();
            handle.Complete();
            m_Red = new FloatParallelConvolutionJobs(channels[0], data[0], handle);
            m_Green = new FloatParallelConvolutionJobs(channels[1], data[1], handle);
            m_Blue = new FloatParallelConvolutionJobs(channels[2], data[2], handle);
        }

        public void Dispose()
        {
            m_Red.Dispose();
            m_Green.Dispose();
            m_Blue.Dispose();
        }
    }
}