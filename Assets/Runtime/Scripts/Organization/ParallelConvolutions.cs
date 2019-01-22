using System;
using Unity.Jobs;

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
    
    public class ParallelConvolutionJobs<T> : IDisposable
        where T: struct, IJob
    {
        public readonly ParallelConvolutions<T> Convolutions;
        
        public T[][] JobSequences;
        
        public ParallelConvolutionJobs(ParallelConvolutions<T> convolutions, T[][] jobSequences)
        {
            Convolutions = convolutions;
            JobSequences = jobSequences;
        }

        public JobHandle Schedule(JobHandle dependency)
        {
            return JobSequences.ScheduleParallel(dependency);
        }

        public void Dispose()
        {
            Convolutions.Dispose();
        }
    }
}

