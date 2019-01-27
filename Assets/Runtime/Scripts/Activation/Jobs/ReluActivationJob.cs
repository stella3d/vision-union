using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion
{
    [BurstCompile]
    public struct BiasedReluActivationSelfJob : IJobParallelFor
    {
        public float Bias;
        
        public NativeArray<float> Input;

        public BiasedReluActivationSelfJob(NativeArray<float> input, float bias = 0f)
        {
            Input = input;
            Bias = bias;
        }

        public void Execute(int index)
        {
            Input[index] = math.max(Input[index] + Bias, 0f);
        }
    }
    
    [BurstCompile]
    public struct BiasedReluActivationJob : IJobParallelFor
    {
        public float Bias;
        
        [ReadOnly] public NativeArray<float> Input;
        [WriteOnly] public NativeArray<float> Output;

        public BiasedReluActivationJob(NativeArray<float> input, NativeArray<float> output, float bias = 0f)
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