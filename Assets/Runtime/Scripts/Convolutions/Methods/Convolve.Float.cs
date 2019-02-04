namespace VisionUnion
{
    public static partial class ConvolutionMethods
    {
        public static void Convolve(this Convolution2D<float> convolution, 
            ImageData<byte> image, ImageData<float> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel2D;
            var pad = convolution.Padding;
            
            for (var r = pad.y; r < image.Height - pad.y; r += stride.y)
            {
                var rowIndex = r * imageWidth;
                for (var c = pad.x; c < imageWidth - pad.x; c += stride.x)
                {
                    var i = rowIndex + c;
                    outputBuffer[i] = kernel.Accumulate(image, i);
                }
            }
        }
        
        public static void Convolve(this Convolution2D<float> convolution, 
            ImageData<short> image, ImageData<float> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel2D;
            var pad = convolution.Padding;
            
            for (var r = pad.y; r < image.Height - pad.y; r += stride.y)
            {
                var rowIndex = r * imageWidth;
                for (var c = pad.x; c < imageWidth - pad.x; c += stride.x)
                {
                    var i = rowIndex + c;
                    outputBuffer[i] = kernel.Accumulate(image, i);
                }
            }
        }
        
        public static void Convolve(this Convolution2D<float> convolution, 
            ImageData<float> image, ImageData<float> output)
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