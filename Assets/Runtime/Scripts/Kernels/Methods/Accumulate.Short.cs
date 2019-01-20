namespace VisionUnion
{
    public static partial class KernelMethods
    {   
        public static short Accumulate(this Kernel<short> kernel, 
            ImageData<byte> imageData, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = 0;
            var pixelBuffer = imageData.Buffer;
            var negativeBound = kernel.Bounds.negative;
            var positiveBound = kernel.Bounds.positive;
            for (var y = negativeBound.y; y <= positiveBound.y; y++)
            {
                var rowOffset = y * imageData.Width;
                var rowIndex = centerPixelIndex + rowOffset;
                for (var x = negativeBound.x; x <= positiveBound.x; x++)
                {
                    var pixelIndex = rowIndex + x;
                    var inputPixelValue = pixelBuffer[pixelIndex];
                    var kernelMultiplier = kernel.Data[kernelIndex];
                    sum += inputPixelValue * kernelMultiplier;
                    kernelIndex++;
                }
            }

            return (short)sum;
        }
        
        public static short Accumulate(this Kernel<short> kernel, 
            ImageData<short> imageData, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = 0;
            var pixelBuffer = imageData.Buffer;
            var negativeBound = kernel.Bounds.negative;
            var positiveBound = kernel.Bounds.positive;
            for (var y = negativeBound.y; y <= positiveBound.y; y++)
            {
                var rowOffset = y * imageData.Width;
                var rowIndex = centerPixelIndex + rowOffset;
                for (var x = negativeBound.x; x <= positiveBound.x; x++)
                {
                    var pixelIndex = rowIndex + x;
                    var inputPixelValue = pixelBuffer[pixelIndex];
                    var kernelMultiplier = kernel.Data[kernelIndex];
                    sum += inputPixelValue * kernelMultiplier;
                    kernelIndex++;
                }
            }

            return (short)sum;
        }
        
        public static float Accumulate(this Kernel<short> kernel, 
            ImageData<float> imageData, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = 0f;
            var pixelBuffer = imageData.Buffer;
            var negativeBound = kernel.Bounds.negative;
            var positiveBound = kernel.Bounds.positive;
            for (var y = negativeBound.y; y <= positiveBound.y; y++)
            {
                var rowOffset = y * imageData.Width;
                var rowIndex = centerPixelIndex + rowOffset;
                for (var x = negativeBound.x; x <= positiveBound.x; x++)
                {
                    var pixelIndex = rowIndex + x;
                    var inputPixelValue = pixelBuffer[pixelIndex];
                    var kernelMultiplier = kernel.Data[kernelIndex];
                    sum += inputPixelValue * kernelMultiplier;
                    kernelIndex++;
                }
            }

            return (short)sum;
        }
    }
}