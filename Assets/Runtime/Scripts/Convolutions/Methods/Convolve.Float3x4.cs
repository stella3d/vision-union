using Unity.Mathematics;

namespace VisionUnion
{
    public static partial class ConvolutionMethods
    {
        // intended for use with each member of the float3 being a color channel
        public static void Convolve(this Convolution2D<float3x4> convolution, 
            ImageData<float3x4> image, ImageData<float3x4> output)
        {
            var imageWidth = image.Width;
            var outWidth = output.Width;
            var outputBuffer = output.Buffer;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel2D;
            var pad = convolution.Padding;

            var outputRow = 0;
            for (var r = pad.y; r < image.Height - pad.y; r += stride.y)
            {
                var rowIndex = r * imageWidth;
                var outRowIndex = outputRow * outWidth;
                for (var c = pad.x; c < imageWidth - pad.x; c += stride.x)
                {
                    var inIndex = rowIndex + c;
                    var outIndex = outRowIndex + c - pad.x;
                    outputBuffer[outIndex] = kernel.Accumulate(image, inIndex);
                }

                outputRow++;
            }
        }
    }
}