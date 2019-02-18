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
        
        [ReadOnly] public Image<byte> Input;
        [WriteOnly] public Image<float> Output;

        public AveragePoolByteToFloatJob(Image<byte> input, Image<float> output, PoolingOptions options)
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
        
        [ReadOnly] public Image<float> Input;
        [WriteOnly] public Image<float> Output;

        public AveragePoolFloatJob(Image<float> input, Image<float> output, PoolingOptions options)
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