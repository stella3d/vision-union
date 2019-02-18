using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    //[BurstCompile]
    public struct ByteWithShortConvolveJob : IJob
    {
        [ReadOnly] public Convolution2D<short> Convolution;
        [ReadOnly] public Image<byte> Input;
        [WriteOnly] public Image<short> Output;

        public ByteWithShortConvolveJob(Convolution2D<short> convolution, 
            Image<byte> input, Image<short> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }

        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
    
    //[BurstCompile]
    public struct ShortWithShortConvolveJob : IJob
    {
        [ReadOnly] public Convolution2D<short> Convolution;
        [ReadOnly] public Image<short> Input;
        [WriteOnly] public Image<short> Output;

        public ShortWithShortConvolveJob(Convolution2D<short> convolution, 
            Image<short> input, Image<short> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }

        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
    
    //[BurstCompile]
    public struct FloatWithShortConvolveJob : IJob
    {
        [ReadOnly] public Convolution2D<short> Convolution;
        [ReadOnly] public Image<float> Input;
        [WriteOnly] public Image<float> Output;

        public FloatWithShortConvolveJob(Convolution2D<short> convolution, 
            Image<float> input, Image<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }

        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
}