using Unity.Collections;

namespace VisionUnion
{
    public static partial class KernelMethods
    {
        public static short Accumulate(this Kernel<short> kernel, 
            NativeArray<byte> pixelBuffer, int centerPixelIndex,
            int width, int xPad, int yPad)
        {
            var kernelIndex = 0;
            short kernelSum = 0;
            for (var kY = -yPad; kY < yPad; kY++)
            {
                var kRowOffset = kY * width;
                var kRowIndex = centerPixelIndex + kRowOffset;
                for (var kX = -xPad; kX < xPad; kX++)
                {
                    var pixelIndex = kRowIndex + kX;
                    var inputPixelValue = pixelBuffer[pixelIndex];
                    var kernelMultiplier = kernel.Data[kernelIndex];
                    kernelSum += (short)(inputPixelValue * kernelMultiplier);
                    kernelIndex++;
                }
            }

            return kernelSum;
        }
        
        public static float AccumulateFloat(this Kernel<short> kernel, 
            NativeArray<byte> pixelBuffer, int centerPixelIndex,
            int width, int xPad, int yPad)
        {
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

            //convert the byte range of 0-255 into the float range of 0-1
            const float oneOver255 = 0.0039215686f;
            return kernelSum * oneOver255;
        }
        
        public static float Accumulate(this Kernel<float> kernel, 
            NativeArray<byte> pixelBuffer, int centerPixelIndex,
            int width, int xPad, int yPad)
        {
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

            //convert the byte range of 0-255 into the float range of 0-1
            const float oneOver255 = 0.0039215686f;
            return kernelSum * oneOver255;
        }
        
        
    }
}