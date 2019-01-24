using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
    public class FloatParallelConvolutionJobs : 
        ParallelConvolutionJobs<float, FloatWithFloatConvolveJob>
    {
        public FloatParallelConvolutionJobs(ImageData<float> input,
            ParallelConvolutionSequences<float> convolutions) 
            : base(input, convolutions)
        {
        }
        
        public override void InitializeJobs()
        {
            for (int n = 0; n < Images.GetLength(0); n++)
            {
                for (var i = 0; i < Images.GetLength(1); i++)
                {
                    var image = Images[n][i];
                    var sequenceJobs = Jobs[n][i];
                    var previous = InputImage;

                    for (var j = 0; j < sequenceJobs.Length; j++)
                    {
                        var newJob = new FloatWithFloatConvolveJob(Convolutions[n][i, j], previous, image);
                        sequenceJobs[j] = newJob;

                        // we assign each job in the sequence the result of the previous convolution
                        previous = newJob.Output;
                    }
                }
            }
        }
    }
}

