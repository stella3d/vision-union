using Unity.Collections;

namespace VisionUnion
{
    public static partial class KernelMethods
    {
        public static void ConvolveHorizontal(this Kernel2D<byte> kernel,
            Image<byte> image, NativeArray<float> pixelOut,
            int xPad)
        {
            var pixelBuffer = image.Buffer;
            var height = image.Height;
            var width = image.Width;
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
    }
}