using Unity.Collections;
using Unity.Mathematics;

namespace VisionUnion
{
    public static class Operations
    {
        public static void IntegralImage(byte[] GrayscaleTexture, int[] IntegralTexture,
            int width, int height)
        {
            // set the top left pixel by itself so we don't have to branch during iteration.
            // since this is the first pixel, the output and input are the same
            var firstInputPixel = GrayscaleTexture[0];
            IntegralTexture[0] = firstInputPixel;
    
            // do the rest of the top row 
            int previousSum = firstInputPixel;
            for (var w = 1; w < width; w++)
            {
                var localIntensity = GrayscaleTexture[w];
                var summedIntensity = localIntensity + previousSum;
                previousSum = summedIntensity;
                IntegralTexture[w] = summedIntensity;
            }
            
            for (var h = 1; h < height; h++)
            {
                var yIndex = h * width;
                var firstLocalIntensity = GrayscaleTexture[yIndex];
                
                var firstTopIndex = yIndex - width;
                var firstTopSumIntensity = IntegralTexture[firstTopIndex];
    
                var firstSum = firstLocalIntensity + firstTopSumIntensity;
                IntegralTexture[yIndex] = firstSum;
                
                for (int w = 1; w < width; w++)
                {
                    var index = yIndex + w;
                    
                    var localIntensity = GrayscaleTexture[index];
    
                    var leftIndex = index - 1;
                    var topIndex = firstTopIndex + w;
                    var topLeftIndex = topIndex - 1;
    
                    var leftSumIntensity = IntegralTexture[leftIndex];
                    var topSumIntensity = IntegralTexture[topIndex];
                    var topLeftSumIntensity = IntegralTexture[topLeftIndex];
    
                    var summedIntensity = localIntensity + leftSumIntensity + topSumIntensity - topLeftSumIntensity;
                    IntegralTexture[index] = summedIntensity;
                }
            }
        }
        
        public static void Average3x3(int[] Integral, float[] Intensities, int width, int height)
        {
            for (int i = 2; i < height - 1; i += 1)
            {
                var rowIndex = i * width;
                var topYIndex = rowIndex - width * 2;
                var bottomYIndex = rowIndex + width;
                
                // do the first column by itself to avoid index errors from the n-2 later on
                var rightIndex1 = 1 + 1;
                var areaRightColumnStart1 = Integral[topYIndex + rightIndex1];
                var areaBottomRight1 = Integral[bottomYIndex + rightIndex1];
                Intensities[rowIndex + 1] = (areaBottomRight1 - areaRightColumnStart1) / 9f / 255f;
            
                for (int n = 2; n < width - 1; n += 1)
                {
                    var rightIndex = n + 1;
                    var leftIndex = n - 2;
                    var areaTopLeftBound = Integral[topYIndex + leftIndex]; 
                    var areaBottomRowStart = Integral[bottomYIndex + leftIndex];
                    var areaRightColumnStart = Integral[topYIndex + rightIndex];
                    var areaBottomRight = Integral[bottomYIndex + rightIndex];

                    var intensitySum = areaBottomRight + areaTopLeftBound - areaRightColumnStart - areaBottomRowStart;
                    Intensities[rowIndex + n] = intensitySum / 9f / 255f;
                }
            }
        }

        public static void MeanPool(NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut, 
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