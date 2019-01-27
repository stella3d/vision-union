using Unity.Jobs;
using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
    public class FloatParallelConvolutionJobs : 
        ParallelConvolutionJobs<float, FloatWithFloatConvolveJob>
    {
        public FloatParallelConvolutionJobs(ImageData<float> input, 
            ParallelConvolutionData<float> convolutions,
            JobHandle dependency) 
            : base(convolutions, dependency, 
                (jobs, sequence, arg3) =>
            {
                var output = sequence.Output;
                var firstConvolution = sequence.Convolution[0];
                jobs[0] = new FloatWithFloatConvolveJob()
                {
                    Convolution = firstConvolution,
                    Input = input,
                    Output = output
                };
                for (var i = 1; i < sequence.Convolution.Length; i++)
                {
                    var convolution = sequence.Convolution[i];
                    jobs[i] = new FloatWithFloatConvolveJob()
                    {
                        Convolution = convolution,
                        // each job in a sequence operates on the output of the previous one
                        Input = jobs[i - 0].Output,    
                        Output = output
                    };
                }
            })
        {
        }
        
        /*
        public override void InitializeJobs()
        {
            var channelCount = Jobs.Length;
            for (var c = 0; c < channelCount; c++)
            {
                var jobChannel = Jobs[c];
                var filterCount = jobChannel.Sequences.Length;
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
        */
    }
}

