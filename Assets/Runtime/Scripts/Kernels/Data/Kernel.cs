using System;
using Unity.Collections;

namespace VisionUnion
{
    public struct Kernel<T> : IDisposable
        where T: struct
    {
        public readonly int Width;
        public readonly int Height;
        public readonly NativeArray<T> Data;

        public Kernel(T[,] input3D, Allocator allocator = Allocator.Persistent)
        {
            Width = input3D.GetLength(0);
            Height = input3D.GetLength(1);

            Data = new NativeArray<T>(Width * Height, allocator);

            var flatIndex = 0;
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Data[flatIndex] = input3D[y, x];
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

        public T this[int row, int column]
        {
            get
            {
                var index = row * Width + column;
                return Data[index];
            }
            set
            {
                var index = row * Width + column;
                var data = Data;
                data[index] = value;
            }
        }

        public void Dispose()
        {
            Data.Dispose();
        }
    }
}