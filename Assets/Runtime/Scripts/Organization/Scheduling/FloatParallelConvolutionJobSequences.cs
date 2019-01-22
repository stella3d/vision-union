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
            for (var i = 0; i < Images.Length; i++)
            {
                var image = Images[i];
                var sequenceJobs = Jobs[i];
                var previous = InputImage;

                for (var j = 0; j < sequenceJobs.Length; j++)
                {
                    var newJob = new FloatWithFloatConvolveJob(Convolutions[i, j], previous, image);
                    sequenceJobs[j] = newJob;

                    // we assign each job in the sequence the result of the previous convolution
                    previous = newJob.Output;
                }
            }
        }
    }
}

