using Unity.Jobs;
using UnityEngine;

namespace VisionUnion.Organization
{
    public static class IConvolutionJobExtensions
    {
        public static void AssignSequence<T>(this IConvolutionJob<T>[] jobs, 
            ConvolutionSequence<T> sequence, JobHandle dependency)
            where T: struct
        {
            var convolutions = sequence.Convolutions;
            if (convolutions.Length != jobs.Length)
            {
                Debug.LogWarningFormat("{0} convolutions but {1} jobs - numbers must match!",
                    convolutions.Length, jobs.Length);
            }

            for (var i = 0; i < convolutions.Length; i++)
            {
                jobs[i].SetConvolution(convolutions[i]);
            }
        }
        
        public static void AssignSequences<T>(this IConvolutionJob<T>[][] jobs, 
            ConvolutionSequence<T>[] sequences, JobHandle dependency)
            where T: struct
        {
            for (var i = 0; i < sequences.Length; i++)
            {
                jobs[i].AssignSequence(sequences[i], dependency);
            }
        }
        
        public static void AssignSequences<T>(this IConvolutionJob<T>[][] jobs, 
            ParallelConvolutions<T>[] sequences, JobHandle dependency)
            where T: struct
        {
            for (var i = 0; i < sequences.Length; i++)
            {
                var seq = sequences[i];
                foreach (var s in seq.Sequences)
                {
                    s.AssignToJobs(dependency, jobs[i]);
                }
            }
        }
    }
}

