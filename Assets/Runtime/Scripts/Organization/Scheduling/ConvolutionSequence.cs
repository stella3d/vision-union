using System;
using System.Collections;
using System.Collections.Generic;

namespace VisionUnion.Organization
{
    /// <summary>
    /// A set of convolutions
    /// </summary>
    /// <typeparam name="T">The kernel type of the convolution</typeparam>
    public class ConvolutionSequence<T> : IDisposable, IEnumerable<ConvolutionSequence<T>> 
        where T: struct
    {
        public Convolution<T>[] Convolutions;
        
        public int Length => Convolutions.Length;

        public ConvolutionSequence(Convolution<T> convolution)
        {
            Convolutions  = new Convolution<T>[1];
            Convolutions[0] = convolution;
        }
        
        public ConvolutionSequence(Convolution<T>[] convolutions)
        {
            Convolutions = convolutions;
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

        IEnumerator<ConvolutionSequence<T>> IEnumerable<ConvolutionSequence<T>>.GetEnumerator()
        {
            return (IEnumerator<ConvolutionSequence<T>>) Convolutions.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Convolutions.GetEnumerator();
        }
    }
}