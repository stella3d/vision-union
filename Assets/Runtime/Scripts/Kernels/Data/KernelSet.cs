using System;

namespace VisionUnion
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
            this.filterCount = filterCount;
            this.channelCount = channelCount;
            Kernels = new Kernel<T>[kY, kX];
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