using System;

namespace VisionUnion.Organization
{
    public class ParallelConvolutions<T> : IDisposable
        where T: struct
    {
        public readonly ConvolutionSequence<T>[] Sequences;
        
        public ParallelConvolutions(ConvolutionSequence<T> sequence)
        {
            Sequences = new ConvolutionSequence<T>[1];
            Sequences[0] = sequence;
        }
        
        public ParallelConvolutions(ConvolutionSequence<T>[] sequences)
        {
            Sequences = sequences;
        }
        
        public void Dispose()
        {
            foreach (var convolution in Sequences)
            {
                convolution.Dispose();
            }
        }
    }
}

