using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion
{
    [BurstCompile]
    public struct ReluActivationJob : IJobParallelFor
    {
        public NativeArray<float> Buffer;

        public ReluActivationJob(NativeArray<float> input)
        {
            Buffer = input;
        }

        public void Execute(int index)
        {
            Buffer[index] = math.max(Buffer[index], 0f);
        }
    }
    
    [BurstCompile]
    public struct ReluActivationCopyJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float> Input;
        [WriteOnly] public NativeArray<float> Output;

        public ReluActivationCopyJob(NativeArray<float> input, NativeArray<float> output)
        {
            Input = input;
            Output = output;
        }

        public void Execute(int index)
        {
            Output[index] = math.max(Input[index], 0f);
        }
    }
    
    [BurstCompile]
    public struct BiasedReluActivationJob : IJobParallelFor
    {
        public float Bias;
        
        public NativeArray<float> Buffer;

        public BiasedReluActivationJob(NativeArray<float> input, float bias = 0f)
        {
            Buffer = input;
            Bias = bias;
        }

        public void Execute(int index)
        {
            Buffer[index] = math.max(Buffer[index] + Bias, 0f);
        }
    }
    
    [BurstCompile]
    public struct BiasedReluActivationCopyJob : IJobParallelFor
    {
        public float Bias;
        
        [ReadOnly] public NativeArray<float> Input;
        [WriteOnly] public NativeArray<float> Output;

        public BiasedReluActivationCopyJob(NativeArray<float> input, NativeArray<float> output, float bias = 0f)
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