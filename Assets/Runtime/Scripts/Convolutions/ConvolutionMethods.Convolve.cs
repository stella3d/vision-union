using System;
using UnityEngine;

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
            
            Debug.Log("done");
        }
        
        public static void ConvolveIterate(this Convolution<byte> convolution, 
            ImageData<byte> image, ImageData<short> output)
        {
            convolution.Iterate(image, output, (conv, inImage, outImage, index) =>
            {
                var outBuffer = outImage.Buffer;
                outBuffer[index] = conv.Kernel.Accumulate(inImage, index);
            });

            Debug.Log("done wither iterative version");
        }
    }
}