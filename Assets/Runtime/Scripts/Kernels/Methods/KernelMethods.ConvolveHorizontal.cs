using Unity.Collections;

namespace VisionUnion
{
    public static partial class KernelMethods
    {
        public static void ConvolveHorizontal(this Kernel<byte> kernel,
            ImageData<byte> imageData, NativeArray<float> pixelOut,
            int xPad)
        {
            var pixelBuffer = imageData.Buffer;
            var height = imageData.Height;
            var width = imageData.Width;
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