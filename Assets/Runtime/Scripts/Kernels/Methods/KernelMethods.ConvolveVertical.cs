using Unity.Collections;

namespace VisionUnion
{
    public static partial class KernelMethods
    {
        public static void ConvolveVertical(this Kernel2D<byte> kernel, 
            Image<byte> image, NativeArray<float> pixelOut,
            int yPad)
        {
            var pixelBuffer = image.Buffer;
            var width = image.Width;
            var height = image.Height;
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