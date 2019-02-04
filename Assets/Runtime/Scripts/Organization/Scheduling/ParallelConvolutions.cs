using System;
using System.Collections;
using System.Collections.Generic;

namespace VisionUnion.Organization
{
    public class ParallelConvolutions<T> : IDisposable, IEnumerable<ConvolutionSequence<T>>
        where T: struct
    {
        public readonly ConvolutionSequence<T>[] Sequences;

        public int sequenceLength => Sequences.Length > 0 ? Sequences[0].Length : 0;
        
        /// <summary>
        /// The number of parallel filter sequences.  Width & Height come from the image
        /// </summary>
        public int Depth => Sequences.Length;
        
        public ParallelConvolutions(ConvolutionSequence<T> sequence)
        {
            Sequences = new ConvolutionSequence<T>[1];
            Sequences[0] = sequence;
        }
        
        public ParallelConvolutions(ConvolutionSequence<T>[] sequences)
        {
            Sequences = sequences;
        }
        
        public ConvolutionSequence<T> this[int sequence]
        {
            get { return Sequences[sequence]; }
            set { Sequences[sequence] = value; }
        }
        
        public Convolution2D<T> this[int sequence, int sequenceIndex]
        {
            get { return Sequences[sequence][sequenceIndex]; }
            set { Sequences[sequence][sequenceIndex] = value;}
        }
        
        public void Dispose()
        {
            Sequences.Dispose();
        }

        IEnumerator<ConvolutionSequence<T>> IEnumerable<ConvolutionSequence<T>>.GetEnumerator()
        {
            return (IEnumerator<ConvolutionSequence<T>>)Sequences.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Sequences.GetEnumerator();
        }
    }
}

