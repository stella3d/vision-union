using System;
using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    /// <summary>
    /// Represents a 2D convolution kernel
    /// </summary>
    /// <typeparam name="T">The data type of the kernel multiplier</typeparam>
    public struct Kernel<T> : IDisposable
        where T: struct
    {
        public readonly int Width;
        public readonly int Height;
        public readonly KernelBounds Bounds;
        public readonly NativeArray<T> Data;

        public Kernel(T[,] input, Allocator allocator = Allocator.Persistent)
        {
            Width = input.GetLength(0);
            Height = input.GetLength(1);

            Bounds = new KernelBounds(Width, Height);

            Data = new NativeArray<T>(Width * Height, allocator);
            
            var flatIndex = 0;
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Data[flatIndex] = input[y, x];
                    flatIndex++;
                }
            }
        }
        
        public Kernel(T[] input, bool horizontal = true, Allocator allocator = Allocator.Persistent)
        {
            Data = new NativeArray<T>(input, allocator);
            Width = horizontal ? input.Length : 1;
            Height = horizontal ? 1 : input.Length;
            Bounds = new KernelBounds(Width, Height);
        }
        
        public Kernel(int width, int height, Allocator allocator = Allocator.Persistent)
        {
            Width = width;
            Height = height;
            Bounds = new KernelBounds(Width, Height);
            Data = new NativeArray<T>(Width * Height, allocator);
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