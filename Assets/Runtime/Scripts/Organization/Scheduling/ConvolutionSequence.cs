using System;
using System.Collections;

namespace VisionUnion.Organization
{
    /// <summary>
    /// A set of convolutions
    /// </summary>
    /// <typeparam name="T">The kernel type of the convolution</typeparam>
    public class ConvolutionSequence<T> : IDisposable, IEnumerable
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

        public void Dispose()
        {
            Convolutions.Dispose();
        }

        public IEnumerator GetEnumerator()
        {
            return Convolutions.GetEnumerator();
        }
    }
}