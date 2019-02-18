namespace VisionUnion
{
    public static partial class KernelMethods
    {   
        public static short Accumulate(this Kernel2D<short> kernel, 
            Image<byte> image, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = 0;
            var pixelBuffer = image.Buffer;
            var negativeBound = kernel.Bounds.negative;
            var positiveBound = kernel.Bounds.positive;
            for (var y = negativeBound.y; y <= positiveBound.y; y++)
            {
                var rowOffset = y * image.Width;
                var rowIndex = centerPixelIndex + rowOffset;
                for (var x = negativeBound.x; x <= positiveBound.x; x++)
                {
                    var pixelIndex = rowIndex + x;
                    var inputPixelValue = pixelBuffer[pixelIndex];
                    var kernelMultiplier = kernel.Weights[kernelIndex];
                    sum += inputPixelValue * kernelMultiplier;
                    kernelIndex++;
                }
            }

            return (short)sum;
        }
        
        public static short Accumulate(this Kernel2D<short> kernel, 
            Image<short> image, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = 0;
            var pixelBuffer = image.Buffer;
            var negativeBound = kernel.Bounds.negative;
            var positiveBound = kernel.Bounds.positive;
            for (var y = negativeBound.y; y <= positiveBound.y; y++)
            {
                var rowOffset = y * image.Width;
                var rowIndex = centerPixelIndex + rowOffset;
                for (var x = negativeBound.x; x <= positiveBound.x; x++)
                {
                    var pixelIndex = rowIndex + x;
                    var inputPixelValue = pixelBuffer[pixelIndex];
                    var kernelMultiplier = kernel.Weights[kernelIndex];
                    sum += inputPixelValue * kernelMultiplier;
                    kernelIndex++;
                }
            }

            return (short)sum;
        }
        
        public static float Accumulate(this Kernel2D<short> kernel, 
            Image<float> image, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = 0f;
            var pixelBuffer = image.Buffer;
            var negativeBound = kernel.Bounds.negative;
            var positiveBound = kernel.Bounds.positive;
            for (var y = negativeBound.y; y <= positiveBound.y; y++)
            {
                var rowOffset = y * image.Width;
                var rowIndex = centerPixelIndex + rowOffset;
                for (var x = negativeBound.x; x <= positiveBound.x; x++)
                {
                    var pixelIndex = rowIndex + x;
                    var inputPixelValue = pixelBuffer[pixelIndex];
                    var kernelMultiplier = kernel.Weights[kernelIndex];
                    sum += inputPixelValue * kernelMultiplier;
                    kernelIndex++;
                }
            }

            return (short)sum;
        }
    }
}