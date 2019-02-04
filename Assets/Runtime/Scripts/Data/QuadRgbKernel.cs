using System;
using Unity.Collections;
using Unity.Mathematics;

namespace VisionUnion
{
    /// <summary>
    /// Represents 4 RGB convolution kernels
    /// </summary>
    /// <typeparam name="T">The data type of the kernel multiplier</typeparam>
    public struct QuadRgbKernel : IDisposable
    {
        public Kernel2D<float3x4> Kernel; 

        public void Dispose()
        {
            Kernel.Dispose();
        }
    }

    public struct QuadRgbImageData : IDisposable
    {
        ImageData<float3x4> ImageQuad;
        
        public void Dispose()
        {
            ImageQuad.Dispose();
        }
    }
}