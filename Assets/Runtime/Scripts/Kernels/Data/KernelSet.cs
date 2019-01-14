using System;
using System.Collections.Generic;
using Unity.Collections;

namespace BurstVision
{
    public class KernelSet<T> : IDisposable
        where T: struct
    {
        public readonly int filterCount;
        public readonly int channelCount;
        
        public readonly Kernel<T>[,] Kernels;

        public KernelSet(Kernel<T>[,] Kernels)
        {
            filterCount = Kernels.GetLength(0);
            channelCount = Kernels.GetLength(1);
            this.Kernels = Kernels;
        }
        
        public KernelSet(int kX, int kY, int filterCount, int channelCount)
        {
            filterCount = Kernels.GetLength(0);
            channelCount = Kernels.GetLength(1);
            this.Kernels = Kernels;
        }
        
        public void Dispose()
        {
            foreach (var kernel in Kernels)
            {
                kernel.Dispose();
            }
        }
    }
}