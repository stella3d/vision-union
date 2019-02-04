using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace VisionUnion
{
    public static partial class Pool 
    {
        public static void Max(NativeArray<float3> pixelBuffer, NativeArray<float3> pixelOut, 
            int width, int height, 
            int xSize = 2, int ySize = 2,        
            int xStride = 2, int yStride = 2) 
        {
            var startingMax = new float3(float.MinValue, float.MinValue, float.MinValue);
            
            var outputIndex = 0;
            for (var i = ySize - 1; i < height; i += yStride)
            {
                var rowIndex = i * width;
                for (var n = xSize - 1; n < width; n += xStride)
                {
                    var index = rowIndex + n;
                    var poolMax = startingMax;
                    for (var kY = -ySize + 1; kY <= 0; kY++)
                    {
                        var kRowOffset = kY * width;
                        var kRowIndex = index + kRowOffset;
                        for (var kX = -xSize + 1; kX <= 0; kX++)
                        {
                            var value = pixelBuffer[kRowIndex + kX];
                            poolMax.x = math.@select(poolMax.x, value.x, value.x > poolMax.x);
                            poolMax.y = math.@select(poolMax.y, value.y, value.y > poolMax.y);
                            poolMax.z = math.@select(poolMax.z, value.z, value.z > poolMax.z);
                        }
                    }

                    pixelOut[outputIndex] = poolMax;
                    outputIndex++;
                }
            }
        }
        
        public static void Max(ImageData<float3> input, ImageData<float3> output, 
            Vector2Int size, Vector2Int strides)
        {
            var inBuffer = input.Buffer;
            var outBuffer = output.Buffer;
            Max(inBuffer, outBuffer, input.Width, input.Height, size.x, size.y, strides.x, strides.y);
        }
    }
}