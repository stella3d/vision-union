using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace BurstVision
{
    public static class KernelOperations
    {
        public static void Run(NativeArray<byte> pixelBuffer, NativeArray<short> pixelOut,
            Kernel<short> kernel, int width, int height)
        {
            var xPad = (kernel.Width - 1) / 2;
            var yPad = (kernel.Height - 1) / 2;
            Debug.LogFormat("padding: {0}, {1}", xPad, yPad);
            
            for (var r = yPad; r < height - yPad; r++)
            {
                var rowIndex = r * width;
                for (var c = xPad; c < width - xPad; c++)
                {
                    var centerPixelIndex = rowIndex + c;
                    var kernelIndex = 0;
                    var kernelSum = 0;
                    for (var kY = -yPad; kY < yPad; kY++)
                    {
                        var kRowOffset = kY * width;
                        var kRowIndex = centerPixelIndex + kRowOffset;
                        for (var kX = -xPad; kX < xPad; kX++)
                        {
                            var pixelIndex = kRowIndex + kX;
                            var inputPixelValue = pixelBuffer[pixelIndex];
                            var kernelMultiplier = kernel.Data[kernelIndex];
                            kernelSum += inputPixelValue * kernelMultiplier;
                            kernelIndex++;
                        }
                    }

                    kernelSum /= 255;
                    pixelOut[centerPixelIndex] = (short)(kernelSum / 255);
                }
            }
        }
        
        public static void Run(NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut,
            Kernel<short> kernel, int width, int height)
        {
            var xPad = (kernel.Width - 1) / 2;
            var yPad = (kernel.Height - 1) / 2;
            
            for (var r = yPad; r < height - yPad; r++)
            {
                var rowIndex = r * width;
                for (var c = xPad; c < width - xPad; c++)
                {
                    var centerPixelIndex = rowIndex + c;
                    var kernelIndex = 0;
                    var kernelSum = 0f;
                    for (var kY = -yPad; kY < yPad; kY++)
                    {
                        var kRowOffset = kY * width;
                        var kRowIndex = centerPixelIndex + kRowOffset;
                        for (var kX = -xPad; kX < xPad; kX++)
                        {
                            var pixelIndex = kRowIndex + kX;
                            var inputPixelValue = pixelBuffer[pixelIndex];
                            var kernelMultiplier = kernel.Data[kernelIndex];
                            kernelSum += inputPixelValue * kernelMultiplier;
                            kernelIndex++;
                        }
                    }

                    kernelSum /= 255f;
                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
    }
}