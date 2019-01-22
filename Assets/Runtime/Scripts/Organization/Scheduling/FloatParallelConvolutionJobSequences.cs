using VisionUnion.Jobs;

namespace VisionUnion.Organization
{
    public class FloatParallelConvolutionJobSequence : 
        ParallelConvolutionJobSequence<float, FloatWithFloatConvolveJob>
    {
        public FloatParallelConvolutionJobSequence(ImageData<float> input,
            ParallelConvolutionSequences<float> convolutions, 
            ParallelJobSequences<FloatWithFloatConvolveJob> jobs) 
            : base(input, convolutions, jobs)
        {
        }
        
        public void InitializeJobs()
        {
            // TODO - figure out if we can make this generic
            for (var i = 0; i < Images.Length; i++)
            {
                var image = Images[i];
                var sequenceJobs = Jobs[i];
                var previous = InputImage;
                for (var j = 0; j < sequenceJobs.Length; j++)
                {
                    var newJob = new FloatWithFloatConvolveJob(Convolutions[i, j], previous, image);
                    sequenceJobs[j] = newJob;
                    previous = newJob.Output;
                }
            }
        }
    }
}

