using System;
using Unity.Collections;

namespace BurstVision
{
    public struct Kernel<T> : IDisposable
        where T: struct
    {
        public readonly int Width;
        public readonly int Height;
        public readonly NativeArray<T> Data;

        public Kernel(T[,] input2D, Allocator allocator = Allocator.Persistent)
        {
            Width = input2D.GetLength(0);
            Height = input2D.GetLength(1);
            
            Data = new NativeArray<T>(Width * Height, allocator);

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
        
        public Kernel(T[] input, bool horizontal = true, Allocator allocator = Allocator.Persistent)
        {
            Data = new NativeArray<T>(input, allocator);
            Width = horizontal ? input.Length : 1;
            Height = horizontal ? 1 : input.Length;
        }

        public void Dispose()
        {
            Data.Dispose();
        }
    }
}