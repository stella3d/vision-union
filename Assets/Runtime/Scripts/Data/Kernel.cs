using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace VisionUnion
{
    
    public struct KernelBounds
    {
        public Vector2Int negative;
        public Vector2Int positive;

        public override string ToString()
        {
            return string.Format("kernel bounds: {0}, {1}", negative, positive);
        }
    }
    
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

            Bounds = GetBounds(Width, Height);

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
            Bounds = GetBounds(Width, Height);
        }
        
        public Kernel(int width, int height, Allocator allocator = Allocator.Persistent)
        {
            Width = width;
            Height = height;
            Bounds = GetBounds(Width, Height);
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
        
        static KernelBounds GetBounds(int width, int height)
        {
            var wMod2 = width % 2;
            var hMod2 = height % 2;
            var wUnder3 = width < 3;
            var hUnder3 = height < 3;

            var pwOffset = wUnder3 ? width - 1 : width / 2;
            var phOffset = hUnder3 ? height - 1 : height / 2;
            var nwOffset = wUnder3 ? 0 : width / 2 - (1 - wMod2);
            var nhOffset = hUnder3 ? 0 : height / 2 - (1 - hMod2);

            var positiveBound = new Vector2Int(pwOffset, phOffset);
            var negativeBound = new Vector2Int(-nwOffset, -nhOffset);

            return new KernelBounds()
            {
                negative = negativeBound,
                positive = positiveBound
            };
        }

        public void Dispose()
        {
            Data.Dispose();
        }
    }
}