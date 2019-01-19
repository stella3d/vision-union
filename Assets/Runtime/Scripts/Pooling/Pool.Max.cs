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
        
        public static void Max(ImageData<byte> input, ImageData<byte> output, 
            Vector2Int size, Vector2Int strides)
        {
            var width = input.Width;
            var height = input.Height;
            var inputBuffer = input.Buffer;
            var outputBuffer = output.Buffer;
            
            var outputIndex = 0;
            for (var i = size.y - 1; i < height; i += strides.y)
            {
                var rowIndex = i * width;
                for (var n = size.x - 1; n < width; n += strides.x)
                {
                    var index = rowIndex + n;
                    int poolMax = byte.MinValue;
                    for (var kY = -size.y + 1; kY <= 0; kY++)
                    {
                        var kRowOffset = kY * width;
                        var kRowIndex = index + kRowOffset;
                        for (var kX = -size.x + 1; kX <= 0; kX++)
                        {
                            var value = inputBuffer[kRowIndex + kX];
                            poolMax = math.@select(poolMax, value, value > poolMax);
                        }
                    }

                    outputBuffer[outputIndex] = (byte)poolMax;
                    outputIndex++;
                }
            }
        }
    }
}