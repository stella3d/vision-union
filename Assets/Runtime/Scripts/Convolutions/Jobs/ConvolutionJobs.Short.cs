using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    //[BurstCompile]
    public struct ByteWithShortConvolveJob : IJob
    {
        [ReadOnly] public Convolution2D<short> Convolution;
        [ReadOnly] public ImageData<byte> Input;
        [WriteOnly] public ImageData<short> Output;

        public ByteWithShortConvolveJob(Convolution2D<short> convolution, 
            ImageData<byte> input, ImageData<short> output)
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
        [ReadOnly] public ImageData<short> Input;
        [WriteOnly] public ImageData<short> Output;

        public ShortWithShortConvolveJob(Convolution2D<short> convolution, 
            ImageData<short> input, ImageData<short> output)
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
        [ReadOnly] public ImageData<float> Input;
        [WriteOnly] public ImageData<float> Output;

        public FloatWithShortConvolveJob(Convolution2D<short> convolution, 
            ImageData<float> input, ImageData<float> output)
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