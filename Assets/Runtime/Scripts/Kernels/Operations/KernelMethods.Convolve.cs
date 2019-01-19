using Unity.Collections;

namespace VisionUnion
{
    public static partial class KernelMethods
    {
        public static void Convolve(this Kernel<short> kernel, 
            NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut,
            int width, int height, 
            int xStride = 1, int yStride = 1)
        {
            var xPad = (kernel.Width - 1) / 2;
            var yPad = (kernel.Height - 1) / 2;
            
            for (var r = yPad; r < height - yPad; r += yStride)
            {
                var rowIndex = r * width;
                for (var c = xPad; c < width - xPad; c += xStride)
                {
                    var centerPixelIndex = rowIndex + c;
                    var kernelSum = kernel.Accumulate(pixelBuffer, centerPixelIndex, width, xPad, yPad);

                    kernelSum /= 255;
                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
        
        public static void Convolve(this Kernel<float> kernel, 
            NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut,
            int width, int height, 
            int xStride = 1, int yStride = 1)
        {
            var xPad = (kernel.Width - 1) / 2;
            var yPad = (kernel.Height - 1) / 2;
            
            for (var r = yPad; r < height - yPad; r += yStride)
            {
                var rowIndex = r * width;
                for (var c = xPad; c < width - xPad; c += xStride)
                {
                    var centerPixelIndex = rowIndex + c;
                    // TODO make padding and kernel offsets different values
                    var kernelSum = kernel.Accumulate(pixelBuffer, centerPixelIndex, width, xPad, yPad);

                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
    }
}