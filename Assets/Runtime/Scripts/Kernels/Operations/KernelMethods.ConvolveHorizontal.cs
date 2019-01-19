using Unity.Collections;

namespace VisionUnion
{
    public static partial class KernelMethods
    {
        public static void ConvolveHorizontal(this Kernel<short> kernel, 
            NativeArray<float> pixelBuffer, NativeArray<float> pixelOut, 
            int width, int height)
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
                        float kernelMultiplier = kernel.Data[kernelIndex];
                        kernelSum += inputPixelValue * kernelMultiplier;
                        kernelIndex++;
                    }

                    kernelSum /= 255f;
                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
    }
}