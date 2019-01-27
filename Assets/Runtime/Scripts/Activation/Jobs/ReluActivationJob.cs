using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion
{
    [BurstCompile]
    public struct ReluActivationSelfJob : IJobParallelFor
    {
        public NativeArray<float> Input;

        public ReluActivationSelfJob(NativeArray<float> input)
        {
            Input = input;
        }

        public void Execute(int index)
        {
            Input[index] = math.max(Input[index], 0f);
        }
    }
    
    [BurstCompile]
    public struct BiasedReluActivationJob : IJobParallelFor
    {
        public float Bias;
        
        [ReadOnly] public NativeArray<float> Input;
        [WriteOnly] public NativeArray<float> Output;

        public BiasedReluActivationJob(NativeArray<float> input, NativeArray<float> output, float bias)
        {
            Input = input;
            Output = output;
            Bias = bias;
        }

        public void Execute(int index)
        {
            Output[index] = math.max(Input[index] + Bias, 0f);
        }
    }
}