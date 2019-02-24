using System;
using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    /// <summary>
    /// Represents a 2D convolution kernel
    /// </summary>
    /// <typeparam name="T">The data type of the kernel multiplier</typeparam>
    public struct Kernel2D<T> : IDisposable
        where T: struct
    {
        public readonly int Width;
        public readonly int Height;
        public readonly KernelBounds Bounds;
        public NativeArray<T> Weights;

        public Kernel2D(T[,] input, Allocator allocator = Allocator.Persistent)
        {
            Width = input.GetLength(0);
            Height = input.GetLength(1);

            Bounds = new KernelBounds(Width, Height);

            Weights = new NativeArray<T>(Width * Height, allocator);
            
            var flatIndex = 0;
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Weights[flatIndex] = input[y, x];
                    flatIndex++;
                }
            }
        }
        
        public Kernel2D(T[] input, bool horizontal = true, Allocator allocator = Allocator.Persistent)
        {
            Weights = new NativeArray<T>(input, allocator);
            Width = horizontal ? input.Length : 1;
            Height = horizontal ? 1 : input.Length;
            Bounds = new KernelBounds(Width, Height);
        }
        
        public Kernel2D(int width, int height, Allocator allocator = Allocator.Persistent)
        {
            Width = width;
            Height = height;
            Bounds = new KernelBounds(Width, Height);
            Weights = new NativeArray<T>(Width * Height, allocator);
        }

        public T this[int row, int column]
        {
            get
            {
                var index = row * Width + column;
                return Weights[index];
            }
            set
            {
                var index = row * Width + column;
                var data = Weights;
                data[index] = value;
            }
        }

        public T[] GetColumn(int column)
        {
            var columnData = new T[Height];
            for (var r = 0; r < Height; r++)
            {
                columnData[r] = this[r, column];
            }

            return columnData;
        }
        
        public T[] GetRow(int row)
        {
            var rowData = new T[Width];
            var rowIndex = row * Width;
            for (var c = 0; c < Width; c++)
            {
                rowData[c] = Weights[rowIndex + c];
            }

            return rowData;
        }
        
        public T[,] ToMatrix()
        {
            var output = new T[Height,Width];
            var flatIndex = 0;
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    output[y, x] = Weights[flatIndex];
                    flatIndex++;
                }
            }

            return output;
        }

        public void SetWeights(T[,] weights)
        {
            var weightsWidth = weights.GetLength(0);
            var weightsHeight = weights.GetLength(1);
            if (weightsWidth != Width || weightsHeight != Height)
            {
                var message = string.Format("weights matrix must be {0}x{1}, but was {2}x{3}",
                    Width, Height, weightsWidth, weightsHeight);
                
                throw new ArgumentException(message);
            }

            for (var y = 0; y < Height; y++)
            {
                var rowIndex = y * Width;
                for (var x = 0; x < Width; x++)
                {
                    Weights[rowIndex + x] = weights[x, y];
                }
            }
        }

        public void Dispose()
        {
            Weights.DisposeIfCreated();
        }
    }
}