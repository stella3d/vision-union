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

    public struct SpecialKernelFloat3x3 : IDisposable
    {
        public const int Width = 3;
        public const int Height = 3;
        public float3x3 Weights;

        static float3[] tempRows = new float3[3];

        public SpecialKernelFloat3x3(float[,] weights)
        {
            Weights = new float3x3();
            SetWeights(weights);
        }

        public void SetWeights(float[,] weights)
        {
            var weightsWidth = weights.GetLength(0);
            var weightsHeight = weights.GetLength(1);
            
            if (weightsWidth != Width || weightsHeight != Height)
            {
                var message = string.Format("weights matrix must be 3x3, but was {2}x{3}",
                    weightsWidth, weightsHeight);
                
                throw new ArgumentException(message);
            }

            for (var y = 0; y < Height; y++)
            {
                var weightRow = weights.GetRow(y);
                tempRows[y] = new float3(weightRow[0], weightRow[1], weightRow[2]);;
            }
            
            Weights = new float3x3(tempRows[0], tempRows[1], tempRows[2]);
            Debug.LogFormat("special kernel weights: {0}", Weights);
        }
        
        public void Dispose()
        {
        }
    }


    public static class SpecialKernelAccumulateMethods
    {
        public static float3x4 AccumulateFour(this SpecialKernelFloat3x3 kernel, Image<float3> image, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = new float3x4();
            var pixelBuffer = image.Buffer;
            var negativeBound = new int2(-1);
            var positiveBound = new int2(1);
            for (var y = negativeBound.y; y <= positiveBound.y; y++)
            {
                var rowOffset = y * image.Width;
                var rowIndex = centerPixelIndex + rowOffset;

                var kernelMultiplier = kernel.Weights[kernelIndex];
                var wideMultiplier = new float3x3(kernelMultiplier, kernelMultiplier, kernelMultiplier);

                var endIndex = positiveBound.x - 3;
                for (var x = negativeBound.x; x <= endIndex; x += 4)
                {
                    var pixelChunkStartIndex = rowIndex + x;
                    var pw = pixelBuffer[pixelChunkStartIndex];
                    var px = pixelBuffer[pixelChunkStartIndex + 1];
                    var py = pixelBuffer[pixelChunkStartIndex + 2];
                    var pz = pixelBuffer[pixelChunkStartIndex + 3];
                    var fourChunk = new float3x4(pw, px, py, pz);

                    var multiplied = math.mul(wideMultiplier, fourChunk);
                    
                    
                    //var inputPixelValue = pixelBuffer[pixelIndex];
                    //sum += inputPixelValue * kernelMultiplier;
                }

                kernelIndex++;
            }

            return sum;
        }
    }
}