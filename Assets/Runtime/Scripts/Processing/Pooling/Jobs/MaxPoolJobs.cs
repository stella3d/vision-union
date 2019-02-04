using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

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
    
    [BurstCompile]
    public struct MaxPoolFloat3Job : IJob
    {
        public PoolingOptions Options;
        
        [ReadOnly] public ImageData<float3> Input;
        [WriteOnly] public ImageData<float3> Output;

        public MaxPoolFloat3Job(ImageData<float3> input, ImageData<float3> output, PoolingOptions options)
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