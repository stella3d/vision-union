using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct MaxPoolByteJob : IJob
    {
        public PoolingOptions Options;
        
        [ReadOnly] public Image<byte> Input;
        [WriteOnly] public Image<byte> Output;

        public MaxPoolByteJob(Image<byte> input, Image<byte> output, PoolingOptions options)
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
        
        [ReadOnly] public Image<float> Input;
        [WriteOnly] public Image<float> Output;

        public MaxPoolFloatJob(Image<float> input, Image<float> output, PoolingOptions options)
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