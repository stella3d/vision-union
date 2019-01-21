using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using VisionUnion.Organization;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct ByteWithFloatConvolveJob : IConvolutionJob<float>
    {
        [ReadOnly] public Convolution<float> Convolution;
        [ReadOnly] public ImageData<byte> Input;
        [WriteOnly] public ImageData<float> Output;

        public ByteWithFloatConvolveJob(Convolution<float> convolution, 
            ImageData<byte> input, ImageData<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }

        public void SetConvolution(Convolution<float> convolution)
        {
            Convolution = convolution;
        }

        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
    
    [BurstCompile]
    public struct ShortWithFloatConvolveJob : IConvolutionJob<float>
    {
        [ReadOnly] public Convolution<float> Convolution;
        [ReadOnly] public ImageData<short> Input;
        [WriteOnly] public ImageData<float> Output;

        public ShortWithFloatConvolveJob(Convolution<float> convolution, 
            ImageData<short> input, ImageData<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }
        
        public void SetConvolution(Convolution<float> convolution)
        {
            Convolution = convolution;
        }

        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
    
    [BurstCompile]
    public struct FloatWithFloatConvolveJob : IConvolutionJob<float>
    {
        [ReadOnly] public Convolution<float> Convolution;
        [ReadOnly] public ImageData<float> Input;
        [WriteOnly] public ImageData<float> Output;

        public FloatWithFloatConvolveJob(Convolution<float> convolution, 
            ImageData<float> input, ImageData<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }
        
        public void SetConvolution(Convolution<float> convolution)
        {
            Convolution = convolution;
        }

        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
}