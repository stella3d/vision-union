using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
    public class FloatParallelConvolutionJobs : 
        ParallelConvolutionJobs<float, FloatWithFloatConvolveJob>
    {
        public FloatParallelConvolutionJobs(ImageData<float> input,
            ParallelConvolutionData<float> convolutions) 
            : base(input, convolutions)
        {
        }
        
        public override void InitializeJobs()
        {
            var filterCount = Jobs.GetLength(1);
            for (var c = 0; c < Jobs.GetLength(0); c++)
            {
                var jobChannel = Jobs[c];
                
                for (var i = 0; i < filterCount; i++)
                {
                    var sequenceJobs = jobChannel[i];
                    var previous = InputImages[0];

                    for (var j = 0; j < sequenceJobs.Length; j++)
                    {
                        var job = jobChannel[i, j];
                        var conv = job.Convolution;
                        var image = job.Output;
                        var newJob = new FloatWithFloatConvolveJob(conv, previous, image);
                        sequenceJobs[j] = newJob;

                        // we assign each job in the sequence the result of the previous convolution.
                        // this supports both separated kernels as well as general stacks of kernels
                        previous = newJob.Output;
                    }
                }
            }
        }
    }
}

