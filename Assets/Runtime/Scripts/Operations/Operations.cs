using System;
using Unity.Burst;
using Unity.Collections;
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
        
        public static void Average3x3(int[] Integral, int[] Intensities, int width, int height)
        {
            for (int i = 1; i < height - 1; i += 1)
            {
                var rowIndex = i * width;
                var topYIndex = rowIndex - width;
                var bottomYIndex = rowIndex + width;
                
                
                // do the first column by itself to avoid index errors from the n-2 later on
                var rightIndex1 = 1 + 1;
                var areaRightColumnStart1 = Integral[topYIndex + rightIndex1];
                var areaBottomRight1 = Integral[bottomYIndex + rightIndex1];
                Intensities[rowIndex + 1] = (areaBottomRight1 - areaRightColumnStart1) / 9;
            
                for (int n = 2; n < width - 1; n += 1)
                {
                    var rightIndex = n + 1;
                    var leftIndex = n - 2;
                    var areaTopLeftBound = Integral[topYIndex + leftIndex]; 
                    var areaBottomRowStart = Integral[bottomYIndex + leftIndex];
                    var areaRightColumnStart = Integral[topYIndex + rightIndex];
                    var areaBottomRight = Integral[bottomYIndex + rightIndex];

                    var intensitySum = areaBottomRight + areaTopLeftBound - areaRightColumnStart - areaBottomRowStart;
                    //Debug.Log("int sum: " + intensitySum);
                    Intensities[rowIndex + n] = intensitySum / 9;
                }
            }
        }

        public static void Average3x3(NativeArray<int> Integral, NativeArray<float> Intensities, int width, int height)
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
                Intensities[rowIndex + 1] = math.abs((areaBottomRight1 - areaRightColumnStart1) / 9f) / (short.MaxValue * 10000f);
            
                for (int n = 2; n < width - 1; n += 1)
                {
                    var rightIndex = n + 1;
                    var leftIndex = n - 2;
                    var areaTopLeftBound = Integral[topYIndex + leftIndex]; 
                    var areaBottomRowStart = Integral[bottomYIndex + leftIndex];
                    var areaRightColumnStart = Integral[topYIndex + rightIndex];
                    var areaBottomRight = Integral[bottomYIndex + rightIndex];

                    var intensitySum = areaBottomRight + areaTopLeftBound - areaRightColumnStart - areaBottomRowStart;
                    Intensities[rowIndex + n] = math.abs(intensitySum / 9f) / (short.MaxValue * 10000f);
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

        public static void Sobel(NativeArray<byte> pixelBuffer, NativeArray<byte> pixelOut, float threshold, 
            int width, int height, byte overThresholdPixel = Byte.MaxValue)
        {
            for (int i = 1; i < height - 1; i++)
            {
                var rowIndex = i * width;
                
                var firstWindowTopLeftIndex = rowIndex - width;
                var previousTopMiddle = pixelBuffer[firstWindowTopLeftIndex];
                var firstWindowTopMiddleIndex = firstWindowTopLeftIndex + 1;
                var previousTopRight = pixelBuffer[firstWindowTopMiddleIndex];
                
                var firstWindowBottomLeftIndex = rowIndex + width;
                var previousBottomMiddle = pixelBuffer[firstWindowBottomLeftIndex];
                var firstWindowBottomMiddleIndex = firstWindowBottomLeftIndex + 1;
                var previousBottomRight = pixelBuffer[firstWindowBottomMiddleIndex];
                
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

                    var inputPixel = pixelBuffer[index];
                    pixelOut[index] = (byte)math.@select(
                        overThresholdPixel - inputPixel,
                        inputPixel,
                        bt < threshold);
                }
            }
        }
        
        public static void SobelVertical(NativeArray<byte> pixelBuffer, NativeArray<byte> pixelOut, float threshold, 
            int width, int height, byte overThresholdPixel = Byte.MaxValue)
        {
            for (int i = 1; i < height - 1; i++)
            {
                var rowIndex = i * width;
                
                var firstWindowTopLeftIndex = rowIndex - width;
                var previousTopMiddle = pixelBuffer[firstWindowTopLeftIndex];
                var firstWindowTopMiddleIndex = firstWindowTopLeftIndex + 1;
                var previousTopRight = pixelBuffer[firstWindowTopMiddleIndex];
                
                var firstWindowBottomLeftIndex = rowIndex + width;
                var previousBottomMiddle = pixelBuffer[firstWindowBottomLeftIndex];
                var firstWindowBottomMiddleIndex = firstWindowBottomLeftIndex + 1;
                var previousBottomRight = pixelBuffer[firstWindowBottomMiddleIndex];
                
                for (int n = 1; n < width - 1; n++)
                {
                    double bt;
                    var index = rowIndex + n;
                    float y = 0f;
             
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
                    var bottomRight = pixelBuffer[index + width + 1];
                    previousBottomRight = bottomRight;
    
                    y += topLeft;
                    y += topMiddle * 2;
                    y += topRight;
                    y -= bottomLeft;
                    y -= bottomMiddle * 2;
                    y -= bottomRight;
            
                    //total intensity value for this pixel neighborhood
                    bt = y;

                    var inputPixel = pixelBuffer[index];
                    pixelOut[index] = (byte)math.@select(
                        overThresholdPixel - inputPixel,
                        inputPixel,
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
        
        // does downsample?
        public static void MaxPool2x2(NativeArray<byte> pixelBuffer, NativeArray<byte> pixelOut, 
            int width, int height)
        {
            int outIndex = 0;
            for (int i = 1; i < height; i += 2)
            {
                var rowIndex = i * width;
                for (int n = 1; n < width; n += 2)
                {
                    var index = rowIndex + n;
             
                    var previousRowIndex = index - width;
                    var topRight = pixelBuffer[previousRowIndex];
                    var topLeft = pixelBuffer[previousRowIndex - 1];
                    var bottomRight = pixelBuffer[index];
                    var bottomLeft = pixelBuffer[index - 1];

                    var max = topLeft > topRight ? topLeft : topRight;
                    max = bottomLeft > max ? bottomLeft : max;
                    max = bottomRight > max ? bottomRight : max;

                    pixelOut[outIndex] = max;
                    outIndex++;
                }
            }
        }
        
        public static void MaxPool2x2(NativeArray<float> pixelBuffer, NativeArray<float> pixelOut, 
            int width, int height)
        {
            for (var i = 1; i < height - 1; i += 2)
            {
                var rowIndex = i * width;
                for (var n = 1; n < width - 1; n += 2)
                {
                    var index = rowIndex + n;
             
                    var previousRowIndex = index - width;
                    var topRight = pixelBuffer[previousRowIndex];
                    var topLeft = pixelBuffer[previousRowIndex - 1];
                    var bottomRight = pixelBuffer[index];
                    var bottomLeft = pixelBuffer[index - 1];

                    var max = topLeft > topRight ? topLeft : topRight;
                    max = bottomLeft > max ? bottomLeft : max;
                    max = bottomRight > max ? bottomRight : max;

                    pixelOut[previousRowIndex] = max;
                    pixelOut[previousRowIndex - 1] = max;
                    pixelOut[index] = max;
                    pixelOut[index - 1] = max;
                }
            }
        }
        
        public static void MeanPool2x2(NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut, 
            int width, int height, 
            int xStride = 2, int yStride = 2) // default stride is non-overlapping
        {
            var outputIndex = 0;
            for (var i = 1; i < height; i += yStride)
            {
                var rowIndex = i * width;
                for (var n = 1; n < width; n += xStride)
                {
                    var index = rowIndex + n;
             
                    var previousRowIndex = index - width;
                    var topRight = pixelBuffer[previousRowIndex];
                    var topLeft = pixelBuffer[previousRowIndex - 1];
                    var bottomRight = pixelBuffer[index];
                    var bottomLeft = pixelBuffer[index - 1];

                    var mean = (topLeft + topRight + bottomLeft + bottomRight) * 0.25f;

                    pixelOut[outputIndex] = mean;
                    outputIndex++;
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
                    var poolIndex = 0;
                    var poolSum = 0f;
                    for (var kY = -ySize + 1; kY <= 0; kY++)
                    {
                        var kRowOffset = kY * width;
                        var kRowIndex = index + kRowOffset;
                        for (var kX = -xSize + 1; kX <= 0; kX++)
                        {
                            var pixelIndex = kRowIndex + kX;
                            poolSum += pixelBuffer[pixelIndex];
                            poolIndex++;
                        }
                    }

                    var average = poolSum / poolSize;
                    pixelOut[outputIndex] = average;
                    outputIndex++;
                }
            }
        }

        public static void SobelCombine(NativeArray<float> xImage, NativeArray<float> yImage,
            NativeArray<float> combined, float threshold)
        {
            for (int i = 0; i < xImage.Length; i++)
            {
                var x = xImage[i];
                var y = yImage[i];
                
                var value = math.sqrt(x * x + y * y);
                //var value = math.abs(x) + math.abs(y);
                combined[i] = math.select(0f, 1f, value / 2f > threshold);
            }
        }
    }
    
}