using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion
{
    [BurstCompile]
    public struct Relu6ActivationJob : IJobParallelFor
    {
        public NativeArray<float> Buffer;

        public Relu6ActivationJob(NativeArray<float> input)
        {
            Buffer = input;
        }

        public void Execute(int index)
        {
            Buffer[index] = math.min(math.max(Buffer[index], 0f), 6f);
        }
    }
    
    [BurstCompile]
    public struct Relu6ActivationCopyJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float> Input;
        [WriteOnly] public NativeArray<float> Output;

        public Relu6ActivationCopyJob(NativeArray<float> input, NativeArray<float> output)
        {
            Input = input;
            Output = output;
        }

        public void Execute(int index)
        {
            Output[index] = math.min(math.max(Input[index], 0f), 6f);
        }
    }
    
    [BurstCompile]
    public struct BiasedRelu6ActivationCopyJob : IJobParallelFor
    {
        public float Bias;
        
        [ReadOnly] public NativeArray<float> Input;
        [WriteOnly] public NativeArray<float> Output;

        public BiasedRelu6ActivationCopyJob(NativeArray<float> input, NativeArray<float> output, float bias = 0f)
        {
            Input = input;
            Output = output;
            Bias = bias;
        }

        public void Execute(int index)
        {
            Output[index] = math.min(math.max(Input[index] + Bias, 0f), 6f);
        }
    }
    
    [BurstCompile]
    public struct BiasedRelu6ActivationJob : IJobParallelFor
    {
        public float Bias;
        
        public NativeArray<float> Buffer;

        public BiasedRelu6ActivationJob(NativeArray<float> input, float bias = 0f)
        {
            Buffer = input;
            Bias = bias;
        }

        public void Execute(int index)
        {
            Buffer[index] = math.min(math.max(Buffer[index] + Bias, 0f), 6f);
        }
    }
}