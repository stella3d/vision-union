using System;
using System.Collections;

namespace VisionUnion.Organization
{
    public class ParallelConvolutionSequences<T> : IDisposable, IEnumerable
        where T: struct
    {
        public readonly ConvolutionSequence<T>[] Sequences;
        
        public int Width => Sequences.Length;
        
        public ParallelConvolutionSequences(ConvolutionSequence<T> sequence)
        {
            Sequences = new ConvolutionSequence<T>[1];
            Sequences[0] = sequence;
        }
        
        public ParallelConvolutionSequences(ConvolutionSequence<T>[] sequences)
        {
            Sequences = sequences;
        }
        
        public Convolution<T> this[int sequence, int sequenceIndex]
        {
            get { return Sequences[sequence].Convolutions[sequenceIndex]; }
            set { Sequences[sequence].Convolutions[sequenceIndex] = value;}
        }
        
        public void Dispose()
        {
            Sequences.Dispose();
        }

        public IEnumerator GetEnumerator()
        {
            return Sequences.GetEnumerator();
        }
    }
}

