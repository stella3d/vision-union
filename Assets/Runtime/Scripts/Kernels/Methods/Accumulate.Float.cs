using Unity.Mathematics;

namespace VisionUnion
{
    public static partial class KernelMethods
    {   
        public static float Accumulate(this Kernel2D<float> kernel, 
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

            return sum;
        }
        
        public static float Accumulate(this Kernel2D<float> kernel, 
            Image<short> image, int centerPixelIndex)
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

            //convert from 0-255 byte range to 0-1 float range
            const float oneOver255 = 0.0039215686f;
            return sum * oneOver255;
        }
        
        public static float Accumulate(this Kernel2D<float> kernel, 
            Image<byte> image, int centerPixelIndex)
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

            //convert from 0-255 byte range to 0-1 float range
            const float oneOver255 = 0.0039215686f;
            return sum * oneOver255;
        }
        
        public static float3 Accumulate(this Kernel2D<float> kernel, 
            Image<float3> image, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = new float3();
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

            return sum;
        }
        
        public static float3 AccumulateVector3(this Kernel2D<float> kernel, 
            Image<float3> image, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = new float3();
            var pixels = image.Buffer;
            var negativeBound = kernel.Bounds.negative;
            var positiveBound = kernel.Bounds.positive;
            for (var y = negativeBound.y; y <= positiveBound.y; y++)
            {
                var rowOffset = y * image.Width;
                var rowIndex = centerPixelIndex + rowOffset;
                var rowVec = new float3x3(pixels[rowIndex - 1], pixels[rowIndex], pixels[rowIndex + 1]);

                var row = (y + 1);
                var kernelRowVec = new float3(kernel.Weights[row * 3], kernel.Weights[row * 3 + 1], kernel.Weights[row * 3 + 2]);
                sum += math.mul(rowVec, kernelRowVec);
            }

            return sum;
        }
        

        
        public static float3 Accumulate(this Kernel2D<float3> kernel, 
            Image<float3> image, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = new float3();
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

            return sum;
        }
        
        public static float3 Accumulate(this Kernel2D<float2> kernel, 
            Image<float3> image, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = new float3();
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
                    var inputPixel = pixelBuffer[pixelIndex];
                    // this one differs from the rest, still experimental
                    var kernelMultiplier = kernel.Weights[kernelIndex];
                    sum += inputPixel * kernelMultiplier.x;
                    sum += inputPixel * kernelMultiplier.y;
                    kernelIndex++;
                }
            }

            return sum;
        }
    }
}