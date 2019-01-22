﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace VisionUnion.Organization
{
    public class ParallelConvolutionSequences<T> : IDisposable, IEnumerable<ConvolutionSequence<T>>
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
        
        public ConvolutionSequence<T> this[int sequence]
        {
            get { return Sequences[sequence]; }
            set { Sequences[sequence] = value; }
        }
        
        public Convolution<T> this[int sequence, int sequenceIndex]
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

