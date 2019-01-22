using System;
using System.Collections;
using Unity.Jobs;

namespace VisionUnion.Organization
{
    /// <summary>
    /// A set of convolutions
    /// </summary>
    /// <typeparam name="T">The kernel type of the convolution</typeparam>
    public class ConvolutionSequence<T> : IDisposable, IEnumerable
        where T: struct
    {
        // TODO - create JobSequence parallel to this
        public Convolution<T>[] Convolutions;

        public JobHandle Handle { get; private set; }
        
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
            foreach (var convolution in Convolutions)
            {
                convolution.Dispose();
            }
        }

        public IEnumerator GetEnumerator()
        {
            return Convolutions.GetEnumerator();
        }
    }
}