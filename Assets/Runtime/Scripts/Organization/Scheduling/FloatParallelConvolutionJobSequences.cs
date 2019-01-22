using System;
using Unity.Jobs;
using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
    public abstract class ParallelConvolutionJobSequence<TData, TJob> : IDisposable
        where TData: struct
        where TJob: struct, IJob
    {
        public readonly ParallelConvolutionSequences<TData> ConvolutionSequences;

        public readonly ParallelJobSequences<TJob> JobSequences;

        public readonly ImageData<float>[] Images;
        
        protected ParallelConvolutionJobSequence(ParallelConvolutionSequences<TData> convolutionSequences, 
            ParallelJobSequences<TJob> jobSequences)
        {
            ConvolutionSequences = convolutionSequences;
            JobSequences = jobSequences;
            Images = new ImageData<float>[jobSequences.Width];
        }

        public void Dispose()
        {
            ConvolutionSequences.Dispose();
        }
    }
    
    public abstract class FloatParallelConvolutionJobSequence<TData> : 
        ParallelConvolutionJobSequence<float, FloatWithFloatConvolveJob>
        where TData: struct
    {
        protected FloatParallelConvolutionJobSequence(ParallelConvolutionSequences<float> convolutionSequences, 
            ParallelJobSequences<FloatWithFloatConvolveJob> jobSequences) 
            : base(convolutionSequences, jobSequences)
        {
        }

        public void SetupJobs()
        {
            
        }

        public JobHandle Schedule(JobHandle dependency)
        {
            /*
            // TODO - figure out if we can make this generic
            var sequenceOne = ConvolutionSequences.Sequences[0];
            var jobs = new FloatWithFloatConvolveJob[1];

			var padded = new ImageData<float>();
            for (var j = 0; j < jobs.Length; j++)
            {
                jobs[j] = new FloatWithFloatConvolveJob(sequenceOne.Convolutions[0],
                    padded, m_ConvolvedDataOne);
            }

			*/
            return new JobHandle();
        }

        public void Dispose()
        {
            ConvolutionSequences.Dispose();
        }
    }
}

