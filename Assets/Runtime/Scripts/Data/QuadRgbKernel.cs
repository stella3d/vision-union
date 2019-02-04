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
        
        public static QuadRgbImageData Combine(ImageData<float3> a, ImageData<float3> b, 
            ImageData<float3> c, ImageData<float3> d, Allocator allocator = Allocator.Persistent)
        {
            var output = new QuadRgbImageData();
            var imageQuad = new ImageData<float3x4>(a.Width, a.Height, allocator);
            var quadBuffer = imageQuad.Buffer;
            
            for (var i = 0; i < a.Buffer.Length; i++)
            {
                quadBuffer[i] = new float3x4(a.Buffer[i], b.Buffer[i], c.Buffer[i], d.Buffer[i]);
            }

            output.ImageQuad = imageQuad;
            return output;
        }
    }
}