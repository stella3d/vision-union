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
    }
}

