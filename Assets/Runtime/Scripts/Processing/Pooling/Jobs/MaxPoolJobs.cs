using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct MaxPoolByteJob : IJob
    {
        public PoolingOptions Options;
        
        [ReadOnly] public ImageData<byte> Input;
        [WriteOnly] public ImageData<byte> Output;

        public MaxPoolByteJob(ImageData<byte> input, ImageData<byte> output, PoolingOptions options)
        {
            Input = input;
            Output = output;
            Options = options;
        }

        public void Execute()
        {
            Pool.Max(Input, Output, Options.Size, Options.Strides);
        }
    }
    
    [BurstCompile]
    public struct MaxPoolFloatJob : IJob
    {
        public PoolingOptions Options;
        
        [ReadOnly] public ImageData<float> Input;
        [WriteOnly] public ImageData<float> Output;

        public MaxPoolFloatJob(ImageData<float> input, ImageData<float> output, PoolingOptions options)
        {
            Input = input;
            Output = output;
            Options = options;
        }

        public void Execute()
        {
            Pool.Max(Input, Output, Options.Size, Options.Strides);
        }
    }
}