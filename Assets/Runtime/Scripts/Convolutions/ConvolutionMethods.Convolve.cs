namespace VisionUnion
{
    public static partial class ConvolutionMethods
    {
        public static void Convolve(this Convolution<byte> convolution, 
            ImageData<byte> image, ImageData<short> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel;
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
        
        public static void Convolve(this Convolution<byte> convolution, 
            ImageData<short> image, ImageData<short> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel;
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
        
        public static void Convolve(this Convolution<short> convolution, 
            ImageData<byte> image, ImageData<short> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel;
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
        
        public static void Convolve(this Convolution<float> convolution, 
            ImageData<byte> image, ImageData<float> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel;
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
        
        public static void Convolve(this Convolution<float> convolution, 
            ImageData<short> image, ImageData<float> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel;
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
        
        public static void Convolve(this Convolution<float> convolution, 
            ImageData<float> image, ImageData<float> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel;
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
        
        public static void Convolve(this Convolution<short> convolution, 
            ImageData<float> image, ImageData<float> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel;
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
        
        public static void Convolve(this Convolution<short> convolution, 
            ImageData<short> image, ImageData<short> output)
        {
            var outputBuffer = output.Buffer;
            var imageWidth = image.Width;
            var stride = convolution.Stride;
            var kernel = convolution.Kernel;
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
    }
}