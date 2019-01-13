using Unity.Collections;
using Unity.Mathematics;

namespace BurstVision
{
    public static partial class KernelMethods
    {
        public static void Convolve(this Kernel<short> kernel, 
            NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut,
            int width, int height, uint xStride = 1, uint yStride = 1)
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
                    var kernelSum = 0;
                    for (var kY = -yPad; kY < yPad; kY++)
                    {
                        var kRowOffset = kY * width;
                        var kRowIndex = centerPixelIndex + kRowOffset;
                        for (var kX = -xPad; kX < xPad; kX++)
                        {
                            var pixelIndex = kRowIndex + kX;
                            var kernelMultiplier = kernel.Data[kernelIndex];

                            var value = (short) math.@select(0, pixelBuffer[pixelIndex] * kernelMultiplier,
                                kernelMultiplier != 0);
                            
                            kernelSum += value;
                            kernelIndex++;
                        }
                    }

                    kernelSum /= 255;
                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
        
        public static void Convolve(this Kernel<float> kernel, 
            NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut,
            int width, int height, uint xStride = 1, uint yStride = 1)
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
                            float kernelMultiplier = kernel.Data[kernelIndex];
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