using System;
using Unity.Collections;
using Unity.Mathematics;
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
        public struct Bounds
        {
            public Vector2Int negative;
            public Vector2Int positive;

            public override string ToString()
            {
                return string.Format("kernel bounds: +{0}, -{1}", positive, negative);
            }
        }

        public readonly int Width;
        public readonly int Height;
        public readonly NativeArray<T> Data;

        public Kernel(T[,] input, Allocator allocator = Allocator.Persistent)
        {
            Width = input.GetLength(0);
            Height = input.GetLength(1);

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
        }
        
        public Kernel(int width, int height, Allocator allocator = Allocator.Persistent)
        {
            Width = width;
            Height = height;
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
        
        public Bounds GetBounds()
        {
            var wMod2 = Width % 2;
            var hMod2 = Height % 2;
            var wUnder3 = Width < 3;
            var hUnder3 = Height < 3;

            var pwOffset = wUnder3 ? Width : Width / 2;
            var phOffset = hUnder3 ? Height : Height / 2;
            var nwOffset = wUnder3 ? 0 : Width / 2 - (1 - wMod2);
            var nhOffset = hUnder3 ? 0 : Height / 2 - (1 - hMod2);

            var positiveBound = new Vector2Int(pwOffset, phOffset);
            var negativeBound = new Vector2Int(-nwOffset, -nhOffset);

            return new Bounds()
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