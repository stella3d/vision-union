using Unity.Mathematics;

namespace VisionUnion
{
    public static partial class KernelMethods
    {   
        // intended for use with each member of the float3 being a color channel : x=r, y=g, z=b
        public static float3 Accumulate(this Kernel2D<float3> kernel, 
            ImageData<float3> imageData, int centerPixelIndex)
        {
            var kernelIndex = 0;
            var sum = new float3();
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
                    var kernelMultiplier = kernel.Weights[kernelIndex];
                    sum += inputPixelValue * kernelMultiplier;
                    kernelIndex++;
                }
            }

            return sum;
        }
    }
}