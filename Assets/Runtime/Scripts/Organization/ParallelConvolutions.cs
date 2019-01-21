using System;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Organization
{
    public class ParallelConvolutions<T> : IDisposable
        where T: struct
    {
        public readonly ConvolutionSequence<T>[] Sequences;

        static readonly NativeList<JobHandle> k_ParallelHandles = new NativeList<JobHandle>(8, Allocator.Persistent);
        
        public ParallelConvolutions(ConvolutionSequence<T> sequence)
        {
            Sequences = new ConvolutionSequence<T>[1];
            Sequences[0] = sequence;
        }
        
        public ParallelConvolutions(ConvolutionSequence<T>[] sequences)
        {
            Sequences = sequences;
        }
        
        public JobHandle ScheduleParallelConvolve()
        {
            var entryHandle = new JobHandle();
            entryHandle.Complete();
            
            k_ParallelHandles.Clear();
            foreach (var sequence in Sequences)
            {
                //k_ParallelHandles.Add(sequence.ScheduleSequence(entryHandle));
            }

            var combinedHandle = JobHandle.CombineDependencies(k_ParallelHandles);
            return combinedHandle;
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

