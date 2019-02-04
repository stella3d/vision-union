using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    // representing byte averages in a byte will result in precision loss usually,
    // so we may transform it to a float output
    [BurstCompile]
    public struct AveragePoolByteToFloatJob : IJob
    {
        public PoolingOptions Options;
        
        [ReadOnly] public ImageData<byte> Input;
        [WriteOnly] public ImageData<float> Output;

        public AveragePoolByteToFloatJob(ImageData<byte> input, ImageData<float> output, PoolingOptions options)
        {
            Input = input;
            Output = output;
            Options = options;
        }

        public void Execute()
        {
            Pool.Average(Input, Output, Options);
        }
    }
    
    [BurstCompile]
    public struct AveragePoolFloatJob : IJob
    {
        public PoolingOptions Options;
        
        [ReadOnly] public ImageData<float> Input;
        [WriteOnly] public ImageData<float> Output;

        public AveragePoolFloatJob(ImageData<float> input, ImageData<float> output, PoolingOptions options)
        {
            Input = input;
            Output = output;
            Options = options;
        }

        public void Execute()
        {
            Pool.Average(Input, Output, Options);
        }
    }
}