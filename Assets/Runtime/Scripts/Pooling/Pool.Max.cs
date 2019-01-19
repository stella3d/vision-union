using Unity.Collections;
using Unity.Mathematics;

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
    }
}