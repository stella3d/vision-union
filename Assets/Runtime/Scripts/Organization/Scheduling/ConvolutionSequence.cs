using System;
using System.Collections;
using System.Collections.Generic;

namespace VisionUnion.Organization
{
    /// <summary>
    /// A set of convolutions that runs serially, each operating on the output of the previous
    /// </summary>
    /// <typeparam name="T">The kernel type of the convolution</typeparam>
    public class ConvolutionSequence<T> : IDisposable, IEnumerable<Convolution<T>> 
        where T: struct
    {
        public Convolution<T>[] Convolutions;
        
        public int Length => Convolutions.Length;
        
        public Convolution<T> Last => Convolutions[Convolutions.Length - 1];

        public ConvolutionSequence(Convolution<T> convolution)
        {
            Convolutions  = new Convolution<T>[1];
            Convolutions[0] = convolution;
        }
        
        public ConvolutionSequence(Convolution<T>[] convolutions)
        {
            Convolutions = convolutions;
        }
        
        public ConvolutionSequence(T[,] kernel, int stride = 1, int pad = 1)
        {
            Convolutions = new [] { new Convolution<T>(kernel, stride, pad) };
        }
        
        public Convolution<T> this[int index]
        {
            get { return Convolutions[index]; }
            set { Convolutions[index] = value;}
        }

        public void Dispose()
        {
            Convolutions.Dispose();
        }

        IEnumerator<Convolution<T>> IEnumerable<Convolution<T>>.GetEnumerator()
        {
            return (IEnumerator<Convolution<T>>) Convolutions.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Convolutions.GetEnumerator();
        }
    }
}