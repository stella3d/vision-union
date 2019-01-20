using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace VisionUnion
{
    public static class Pad
    {
        public static ImageData<T> Image<T>(ImageData<T> input, Padding padding, PadMode mode, 
            T constantValue = default(T), Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            var output = default(ImageData<T>);
            switch (mode)
            {
                // constant is the only method supported so far
                case PadMode.Constant:
                    output = Constant(input, padding, constantValue, allocator);
                    break;
            }

            return output;
        }
        
        public static ImageData<T> Image<T>(ImageData<T> input, Padding padding, 
            ConvolutionPadMode mode = ConvolutionPadMode.Same,
            T constantValue = default(T), Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            var output = default(ImageData<T>);
            switch (mode)
            {
                case ConvolutionPadMode.Same:
                    output = Constant(input, padding, constantValue, allocator);
                    break;
                case ConvolutionPadMode.Valid:
                    output = Constant(input, padding, constantValue, allocator);
                    break;
            }

            return output;
        }

        public static Padding GetSamePad<TInput, TConvolution>(ImageData<TInput> input, 
            Convolution<TConvolution> convolution)
            where TInput: struct
            where TConvolution: struct
        {
            var strides = convolution.Stride;
            var kernel = convolution.Kernel;
            Debug.LogFormat("same pad input height: {0} , width {1}", input.Height, input.Width);

            var outHeight = (int)math.ceil((float)(input.Height - kernel.Height + 1) / strides.y);
            var outWidth = (int)math.ceil((float)(input.Width - kernel.Width + 1) / strides.x);
            
            Debug.LogFormat("same pad output height: {0} , width {1}", outHeight, outWidth);

            var padding = new Padding();
            var hDiff = input.Height - outHeight;
            var wDiff = input.Width - outWidth;
            if (hDiff % 2 == 0)
            {
                var halfDiff = hDiff / 2;
                padding.left += halfDiff;
                padding.right += halfDiff;
            }
            else
            {
                var halfDiff = (int)math.floor(hDiff / 2);
                padding.left += halfDiff;
                padding.right += halfDiff + 1;
            }
            if (wDiff % 2 == 0)
            {
                var halfDiff = wDiff / 2;
                padding.top += halfDiff;
                padding.bottom += halfDiff;
            }
            else
            {
                var halfDiff = (int)math.floor(wDiff / 2);
                padding.top += halfDiff;
                padding.bottom += halfDiff + 1;
            }


            return padding;
        }

        public static ImageData<T> Constant<T>(ImageData<T> input, Padding pad, T value = default(T), 
            Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            var newWidth = input.Width + pad.left + pad.right;
            var newHeight = input.Height + pad.top + pad.bottom;
           
            var newImage = new ImageData<T>(newWidth, newHeight, allocator);
            var outBuffer = newImage.Buffer;
            var inBuffer = input.Buffer;

            var inputIndex = 0;
            var outIndex = 0;
            for (var i = 0; i < pad.top; i++)                                     // top pad
            {
                for (var c = 0; c < newImage.Width; c++)
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }

            var contentRowEnd = newHeight - pad.bottom;
            for (var r = pad.top; r < contentRowEnd; r++)
            {
                var contentColumnEnd = newWidth - pad.right;
                for (var i = 0; i < pad.left; i++)                               // left pad
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
                for (var i = pad.left; i < contentColumnEnd; i++)                // original content
                {
                    outBuffer[outIndex] = inBuffer[inputIndex];
                    outIndex++;
                    inputIndex++;
                }
                for (var i = contentColumnEnd; i < newWidth; i++)                // right pad
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }
            
            for (var r = contentRowEnd; r < newHeight; r++)                      // bottom pad
            {
                for (var c = 0; c < newImage.Width; c++)
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }

            return newImage;
        }
    }
}