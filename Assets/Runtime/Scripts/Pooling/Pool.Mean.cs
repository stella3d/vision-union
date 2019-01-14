using Unity.Collections;

namespace BurstVision
{
    public static partial class Pool
    {
        public static void Mean(NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut, 
            int width, int height, 
            int xSize = 2, int ySize = 2,         // default is a 2x2 downsampling
            int xStride = 2, int yStride = 2) 
        {
            var outputIndex = 0;
            var poolSize = xSize * ySize;
            for (var i = ySize - 1; i < height; i += yStride)
            {
                var rowIndex = i * width;
                for (var n = xSize - 1; n < width; n += xStride)
                {
                    var index = rowIndex + n;
                    var poolSum = 0f;
                    for (var kY = -ySize + 1; kY <= 0; kY++)
                    {
                        var kRowOffset = kY * width;
                        var kRowIndex = index + kRowOffset;
                        for (var kX = -xSize + 1; kX <= 0; kX++)
                        {
                            poolSum += pixelBuffer[kRowIndex + kX];
                        }
                    }

                    pixelOut[outputIndex] = poolSum / poolSize;
                    outputIndex++;
                }
            }
        }
    }
}