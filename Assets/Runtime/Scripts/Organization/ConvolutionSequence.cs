using System;
using Unity.Jobs;

namespace VisionUnion.Organization
{
    /// <summary>
    /// A set of convolutions
    /// </summary>
    /// <typeparam name="T">The kernel type of the convolution</typeparam>
    public class ConvolutionSequence<T> : IDisposable
        where T: struct
    {
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
        
        public void AssignToJobs(JobHandle dependency, IConvolutionJob<T>[] jobs)
        {
            for (var i = 0; i < jobs.Length; i++)
            {
                jobs[i].SetConvolution(Convolutions[i]);
            }
        }
    }
}