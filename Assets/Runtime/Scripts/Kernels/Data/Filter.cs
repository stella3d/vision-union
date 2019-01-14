using System;

namespace BurstVision
{
    public struct Filter<T> : IDisposable
        where T: struct
    {
        public readonly int StrideX;
        public readonly int StrideY;
        public readonly Kernel<T> Kernel;

        public Filter(Kernel<T> kernel, int strideX = 1, int strideY = 1)
        {
            StrideX = strideX;
            StrideY = strideY;
            Kernel = kernel;
        }
        
        public void Dispose()
        {
            Kernel.Dispose();
        }
    }
}