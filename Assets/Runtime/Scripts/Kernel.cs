using System;
using Unity.Collections;

namespace BurstVision
{
    
    public struct Kernel : IDisposable
    {
        public readonly int Width;
        public readonly int Height;
        public readonly NativeArray<short> Data;

        public Kernel(short[,] input2D, Allocator allocator = Allocator.Persistent)
        {
            Width = input2D.GetLength(0);
            Height = input2D.GetLength(1);
            
            Data = new NativeArray<short>(Width * Height, allocator);

            var flatIndex = 0;
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Data[flatIndex] = input2D[y, x];
                    flatIndex++;
                }
            }
        }

        public void Dispose()
        {
            Data.Dispose();
        }
    }
    
    public struct PreparedKernel
    {
        public short width;
        public short height;
        public NativeArray<KernelPixel> data;
    }
    
    public struct InputKernelPixel
    {
        public int multiplier;
        public int bufferOffset;
    }

    public struct KernelPixel
    {
        public int multiplier;
        public int bufferOffset;
    }
}