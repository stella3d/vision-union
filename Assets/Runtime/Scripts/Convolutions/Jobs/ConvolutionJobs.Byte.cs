using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct ByteWithByteConvolveJob : IJob
    {
        [ReadOnly] public Convolution<byte> Convolution;
        [ReadOnly] public ImageData<byte> Input;
        [WriteOnly] public ImageData<short> Output;

        public ByteWithByteConvolveJob(Convolution<byte> convolution, 
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
    
    [BurstCompile]
    public struct ShortWithByteConvolveJob : IJob
    {
        [ReadOnly] public Convolution<byte> Convolution;
        [ReadOnly] public ImageData<short> Input;
        [WriteOnly] public ImageData<short> Output;

        public ShortWithByteConvolveJob(Convolution<byte> convolution, 
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
}