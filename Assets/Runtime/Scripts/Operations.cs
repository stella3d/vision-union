using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurstVision
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
            for (int w = 1; w < width; w++)
            {
                var localIntensity = GrayscaleTexture[w];
                var summedIntensity = localIntensity + previousSum;
                previousSum = summedIntensity;
                IntegralTexture[w] = summedIntensity;
            }
            
            for (int h = 1; h < height; h++)
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
    
        public static void Sobel(NativeArray<byte> pixelBuffer, NativeArray<byte> pixelOut, float threshold, 
            int width, int height, byte overThresholdPixel = Byte.MaxValue)
        {
            for (int i = 1; i < height - 1; i++)
            {
                var rowIndex = i * width;
                
                var firstWindowTopLeftIndex = rowIndex - width;
                byte previousTopMiddle = pixelBuffer[firstWindowTopLeftIndex];
                var firstWindowTopMiddleIndex = firstWindowTopLeftIndex + 1;
                byte previousTopRight = pixelBuffer[firstWindowTopMiddleIndex];
                
                var firstWindowBottomLeftIndex = rowIndex - width;
                byte previousBottomMiddle = pixelBuffer[firstWindowBottomLeftIndex];
                var firstWindowBottomMiddleIndex = firstWindowBottomLeftIndex + 1;
                byte previousBottomRight = pixelBuffer[firstWindowBottomMiddleIndex];
                
                for (int n = 1; n < width - 1; n++)
                {
                    double bt;
                    var index = rowIndex + n;
                    float x = 0f, y = 0f;
             
                    // kernel calculations
                    
                    var topLeft = previousTopMiddle;
                    var topMiddle = previousTopRight;
                    previousTopMiddle = topMiddle;
                    // we only have to access the leading pixel in the row each iteration
                    var topRight = pixelBuffer[index - width + 1];
                    previousTopRight = topRight;
                    
                    var leftMiddle = pixelBuffer[index - 1];
                    var rightMiddle = pixelBuffer[index + 1];
                    
                    var bottomLeft = previousBottomMiddle;
                    var bottomMiddle = previousBottomRight;
                    previousBottomMiddle = bottomMiddle;
                    // we only have to access the leading pixel in the row each iteration
                    var bottomRight = pixelBuffer[index + width + 1];
                    previousBottomRight = bottomRight;
    
                    x -= topLeft;
                    x += topRight;
                    x -= leftMiddle * 2;
                    x += rightMiddle * 2;
                    x -= bottomLeft;
                    x += bottomRight;
    
                    y += topLeft;
                    y += topMiddle * 2;
                    y += topRight;
                    y -= bottomLeft;
                    y -= bottomMiddle * 2;
                    y -= bottomRight;
            
                    //total intensity value for this pixel neighborhood
                    bt = math.sqrt(x * x + y * y);
                    
                    pixelOut[index] = (byte)math.@select(
                        overThresholdPixel,
                        (short)pixelBuffer[index],
                        bt < threshold);
                }
            }
        }
        
        public static void Sobel(byte[] pixelBuffer, byte[] pixelOut, float threshold, 
            int width, int height, byte overThresholdPixel = Byte.MaxValue)
        {
            for (int i = 1; i < height - 1; i++)
            {
                var rowIndex = i * width;
                
                for (int n = 1; n < width - 1; n++)
                {
                    double bt;
                    var index = rowIndex + n;
                    float x = 0f, y = 0f;
             
                    //kernel calculations
                    var iMinusWidth = index - width;
                    var topLeft = pixelBuffer[iMinusWidth - 1];
                    var topMiddle = pixelBuffer[iMinusWidth];
                    var topRight = pixelBuffer[iMinusWidth + 1];
                    
                    var leftMiddle = pixelBuffer[index - 1];
                    var rightMiddle = pixelBuffer[index + 1];
                    
                    var iPlusWidth = index + width;
                    var bottomLeft = pixelBuffer[iPlusWidth - 1];
                    var bottomMiddle = pixelBuffer[iPlusWidth];
                    var bottomRight = pixelBuffer[iPlusWidth + 1];
    
                    x -= topLeft;
                    x += topRight;
                    x -= leftMiddle * 2;
                    x += rightMiddle * 2;
                    x -= bottomLeft;
                    x += bottomRight;
    
                    y += topLeft;
                    y += topMiddle * 2;
                    y += topRight;
                    y -= bottomLeft;
                    y -= bottomMiddle * 2;
                    y -= bottomRight;
            
                    //total intensity value for this pixel neighborhood
                    bt = math.sqrt(x * x + y * y);
                    if (bt < threshold)
                    {
                        pixelOut[index] = (byte) math.@select(255 - bt, byte.MinValue, bt < 255);
                    }
                    else
                        pixelOut[index] = overThresholdPixel;
                }
            }
        }
        
        public static void Sobel(NativeArray<int> summedAreaTable, NativeArray<byte> pixelOut, float threshold, 
            int width, int height)
        {
            for (int i = 1; i < height - 1; i++)
            {
                var rowIndex = i * width;
                
                for (int n = 1; n < width - 1; n++)
                {
                    double bt;
                    var index = rowIndex + n;
                    float x = 0f, y = 0f;
             
                    //kernel calculations
                    var iMinusWidth = index - width;
                    var topLeft = summedAreaTable[iMinusWidth - 1];
                    
                    var iPlusWidth = index + width;
                    var bottomLeft = summedAreaTable[iPlusWidth - 1];
                    var bottomRight = summedAreaTable[iPlusWidth + 1];
    
                    var topMiddle = summedAreaTable[iMinusWidth];
                    var topRight = summedAreaTable[iMinusWidth + 1];
                    
                    var leftMiddle = summedAreaTable[index - 1];
                    var rightMiddle = summedAreaTable[index + 1];
                    
                    var bottomMiddle = summedAreaTable[iPlusWidth];
    
                    x -= topLeft;
                    x += topRight;
                    x -= leftMiddle * 2;
                    x += rightMiddle * 2;
                    x -= bottomLeft;
                    x += bottomRight;
    
                    y += topLeft;
                    y += topMiddle * 2;
                    y += topRight;
                    y -= bottomLeft;
                    y -= bottomMiddle * 2;
                    y -= bottomRight;
            
                    //total intensity value for this pixel neighborhood
                    bt = math.sqrt(x * x + y * y);
                    if (bt < threshold)
                    {
                        pixelOut[index] = (byte) math.@select(255 - bt, byte.MinValue, bt < 255);
                    }
                    else
                        pixelOut[index] = byte.MaxValue;
                }
            }
        }
    }
}