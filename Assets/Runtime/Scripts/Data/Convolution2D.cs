using System;
using UnityEngine;

namespace VisionUnion
{
    /// <summary>
    /// A combination of kernel, stride, & padding, everything we need to convolve an image
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Convolution2D<T> : IDisposable
        where T: struct
    {
        public Vector2Int Stride;
        public Vector2Int Padding;
        public Kernel2D<T> Kernel2D;
        
        
        
        public Convolution2D(Kernel2D<T> kernel, int stride = 1, int pad = 1)
        {
            Kernel2D = kernel;
            Stride = new Vector2Int(stride, stride);
            Padding = new Vector2Int(pad, pad);
        }
        
        public Convolution2D(Kernel2D<T> kernel, Vector2Int stride, Vector2Int padding)
        {
            Kernel2D = kernel;
            Stride = stride;
            Padding = padding;
        }
        
        public Convolution2D(T[,] kernel, int stride = 1, int pad = 1)
        {
            Kernel2D = new Kernel2D<T>(kernel);
            Stride = new Vector2Int(stride, stride);
            Padding = new Vector2Int(pad, pad);
        }

        public void Dispose()
        {
            Kernel2D.Dispose();
        }
    }
}