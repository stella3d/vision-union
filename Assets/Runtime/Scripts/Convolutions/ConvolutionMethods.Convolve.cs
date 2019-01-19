namespace VisionUnion
{
    public static partial class ConvolutionMethods
    {
        public static void Convolve(this Convolution<short> convolution, 
            ImageData<byte> image, ImageData<short> output)
        {
            var inputBuffer = image.Buffer;
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var pad = convolution.Padding;
            var strides = convolution.Stride;
            var kernel = convolution.Kernel;

            for (var r = pad.y; r < image.Height - pad.y; r += strides.y)
            {
                var rowIndex = r * imageWidth;
                for (var c = pad.x; c < imageWidth - pad.x; c += strides.x)
                {
                    var i = rowIndex + c;
                    var kernelSum = kernel.Accumulate(inputBuffer, i, imageWidth, pad.x, pad.y);
                    outputBuffer[i] = kernelSum;
                }
            }
        }
    }
}