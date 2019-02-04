using System;
using System.Collections;
using System.Collections.Generic;

namespace VisionUnion.Organization
{
    /// <summary>
    /// A set of convolutions that runs serially, each operating on the output of the previous
    /// </summary>
    /// <typeparam name="T">The kernel type of the convolution</typeparam>
    public class ConvolutionSequence<T> : IDisposable, IEnumerable<Convolution2D<T>> 
        where T: struct
    {
        public readonly Convolution2D<T>[] Convolutions;
        
        public int Length => Convolutions.Length;
        
        public Convolution2D<T> Last => Convolutions[Convolutions.Length - 1];

        public ConvolutionSequence(Convolution2D<T> convolution)
        {
            Convolutions  = new Convolution2D<T>[1];
            Convolutions[0] = convolution;
        }
        
        public ConvolutionSequence(Convolution2D<T>[] convolutions)
        {
            Convolutions = convolutions;
        }
        
        public ConvolutionSequence(T[,] kernel, int stride = 1, int pad = 1)
        {
            Convolutions = new [] { new Convolution2D<T>(kernel, stride, pad) };
        }
        
        public Convolution2D<T> this[int index]
        {
            get { return Convolutions[index]; }
            set { Convolutions[index] = value;}
        }

        public void Dispose()
        {
            Convolutions.Dispose();
        }

        IEnumerator<Convolution2D<T>> IEnumerable<Convolution2D<T>>.GetEnumerator()
        {
            return (IEnumerator<Convolution2D<T>>) Convolutions.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Convolutions.GetEnumerator();
        }
    }
}