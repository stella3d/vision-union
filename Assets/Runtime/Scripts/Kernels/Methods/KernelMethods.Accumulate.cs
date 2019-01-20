
namespace VisionUnion
{
    public static partial class KernelMethods
    {   
        public static short Accumulate(this Kernel<byte> kernel, 
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
        
        public static short Accumulate(this Kernel<byte> kernel, 
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
        
        public static float Accumulate(this Kernel<float> kernel, 
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

            return sum;
        }
        
        public static float Accumulate(this Kernel<float> kernel, 
            ImageData<short> imageData, int centerPixelIndex)
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

            //convert from 0-255 byte range to 0-1 float range
            const float oneOver255 = 0.0039215686f;
            return sum * oneOver255;
        }
        
        public static float Accumulate(this Kernel<float> kernel, 
            ImageData<byte> imageData, int centerPixelIndex)
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

            //convert from 0-255 byte range to 0-1 float range
            const float oneOver255 = 0.0039215686f;
            return sum * oneOver255;
        }
    }
}