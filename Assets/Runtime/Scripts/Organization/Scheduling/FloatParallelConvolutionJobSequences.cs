using Unity.Jobs;
using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
    public class FloatParallelConvolutionJobSequence<TData> : 
        ParallelConvolutionJobSequence<float, FloatWithFloatConvolveJob>
        where TData: struct
    {
        protected FloatParallelConvolutionJobSequence(int width, int height,
            ParallelConvolutionSequences<float> convolutions, 
            ParallelJobSequences<FloatWithFloatConvolveJob> jobs) 
            : 
                base(width, height, convolutions, jobs)
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
    }
}

