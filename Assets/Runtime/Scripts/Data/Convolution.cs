using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisionUnion
{
    /// <summary>
    /// A combination of kernel, stride, & padding, everything we need to convolve an image
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Convolution<T> : IDisposable
        where T: struct
    {
        public Vector2Int Stride;
        public Vector2Int Padding;
        public Kernel<T> Kernel;
        
        public Convolution(Kernel<T> kernel, int stride = 1, int pad = 1)
        {
            Kernel = kernel;
            Stride = new Vector2Int(stride, stride);
            Padding = new Vector2Int(pad, pad);
        }
        
        public Convolution(Kernel<T> kernel, Vector2Int stride, Vector2Int padding)
        {
            Kernel = kernel;
            Stride = stride;
            Padding = padding;
        }

        public void Iterate<TInput, TOutput>(ImageData<TInput> input, ImageData<TOutput> output,
            Action<Convolution<T>, ImageData<TInput>, ImageData<TOutput>, int> action)
            where TInput: struct
            where TOutput: struct
        {
            var imageWidth = input.Width;
            var stride = Stride;
            var pad = Padding;
            for (var r = pad.y; r < input.Height - pad.y; r += stride.y)
            {
                var rowIndex = r * imageWidth;
                for (var c = pad.x; c < imageWidth - pad.x; c += stride.x)
                {
                    action(this, input, output, rowIndex + c);
                }
            }
        }

        public void Dispose()
        {
            Kernel.Dispose();
        }
    }
}