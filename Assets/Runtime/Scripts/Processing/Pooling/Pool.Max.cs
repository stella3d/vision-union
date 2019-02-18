using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace VisionUnion
{
    public static partial class Pool 
    {
        public static void Max(NativeArray<byte> pixelBuffer, NativeArray<byte> pixelOut, 
            int width, int height, 
            int xSize = 2, int ySize = 2,         // default is a 2x2 downsampling
            int xStride = 2, int yStride = 2) 
        {
            var outputIndex = 0;
            for (var i = ySize - 1; i < height; i += yStride)
            {
                var rowIndex = i * width;
                for (var n = xSize - 1; n < width; n += xStride)
                {
                    var index = rowIndex + n;
                    int poolMax = byte.MinValue;
                    for (var kY = -ySize + 1; kY <= 0; kY++)
                    {
                        var kRowOffset = kY * width;
                        var kRowIndex = index + kRowOffset;
                        for (var kX = -xSize + 1; kX <= 0; kX++)
                        {
                            var value = pixelBuffer[kRowIndex + kX];
                            poolMax = math.@select(poolMax, value, value > poolMax);
                        }
                    }

                    pixelOut[outputIndex] = (byte)poolMax;
                    outputIndex++;
                }
            }
        }
        
        public static void Max(Image<byte> input, Image<byte> output, 
            Vector2Int size, Vector2Int strides)
        {
            var inBuffer = input.Buffer;
            var outBuffer = output.Buffer;
            Max(inBuffer, outBuffer, input.Width, input.Height, size.x, size.y, strides.x, strides.y);
        }
        
        public static void Max(NativeArray<float> pixelBuffer, NativeArray<float> pixelOut, 
            int width, int height, 
            int xSize = 2, int ySize = 2,         // default is a 2x2 downsampling
            int xStride = 2, int yStride = 2) 
        {
            var outputIndex = 0;
            for (var i = ySize - 1; i < height; i += yStride)
            {
                var rowIndex = i * width;
                for (var n = xSize - 1; n < width; n += xStride)
                {
                    var index = rowIndex + n;
                    var poolMax = float.MinValue;
                    for (var kY = -ySize + 1; kY <= 0; kY++)
                    {
                        var kRowOffset = kY * width;
                        var kRowIndex = index + kRowOffset;
                        for (var kX = -xSize + 1; kX <= 0; kX++)
                        {
                            var value = pixelBuffer[kRowIndex + kX];
                            poolMax = math.@select(poolMax, value, value > poolMax);
                        }
                    }

                    pixelOut[outputIndex] = poolMax;
                    outputIndex++;
                }
            }
        }
        
        public static void Max(Image<float> input, Image<float> output, 
            Vector2Int size, Vector2Int strides)
        {
            var inBuffer = input.Buffer;
            var outBuffer = output.Buffer;
            Max(inBuffer, outBuffer, input.Width, input.Height, size.x, size.y, strides.x, strides.y);
        }
    }
}