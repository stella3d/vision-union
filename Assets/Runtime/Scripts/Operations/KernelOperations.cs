using Unity.Collections;

namespace VisionUnion
{
    public static class KernelOperations
    {
        public static void RunHorizontal1D(NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut,
            Kernel<short> kernel, int width, int height)
        {
            var xPad = (kernel.Width - 1) / 2;
            
            for (var r = 0; r < height; r++)
            {
                var rowIndex = r * width;
                for (var c = xPad; c < width - xPad; c++)
                {
                    var centerPixelIndex = rowIndex + c;
                    var kernelIndex = 0;
                    var kernelSum = 0f;
                    for (var kX = -xPad; kX < xPad; kX++)
                    {
                        var pixelIndex = centerPixelIndex + kX;
                        var inputPixelValue = pixelBuffer[pixelIndex];
                        float kernelMultiplier = kernel.Weights[kernelIndex];
                        kernelSum += inputPixelValue * kernelMultiplier;
                        kernelIndex++;
                    }

                    kernelSum /= 255f;
                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
        
        public static void RunVertical1D(NativeArray<float> pixelBuffer, NativeArray<float> pixelOut,
            Kernel<short> kernel, int width, int height)
        {
            var yPad = (kernel.Height - 1) / 2;
            
            for (var r = yPad; r < height - yPad; r++)
            {
                var rowIndex = r * width;
                for (var c = 0; c < width; c++)
                {
                    var centerPixelIndex = rowIndex + c;
                    var kernelIndex = 0;
                    var kernelSum = 0f;
                    for (var kY = -yPad; kY < yPad; kY++)
                    {
                        var kRowOffset = kY * width;
                        var pixelIndex = centerPixelIndex + kRowOffset;

                        var inputPixelValue = pixelBuffer[pixelIndex];
                        float kernelMultiplier = kernel.Weights[kernelIndex];
                        kernelSum += inputPixelValue * kernelMultiplier;
                        kernelIndex++;
                    }

                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
    }
}