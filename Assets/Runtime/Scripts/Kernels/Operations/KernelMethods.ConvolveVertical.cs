using Unity.Collections;

namespace VisionUnion
{
    public static partial class KernelMethods
    {
        public static void ConvolveVertical(this Kernel<short> kernel, 
            NativeArray<float> pixelBuffer, NativeArray<float> pixelOut, 
            int width, int height)
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
                        float kernelMultiplier = kernel.Data[kernelIndex];
                        kernelSum += inputPixelValue * kernelMultiplier;
                        kernelIndex++;
                    }

                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
    }
}