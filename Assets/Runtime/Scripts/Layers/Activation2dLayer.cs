using Unity.Collections;
using Unity.Jobs;

namespace BurstVision
{
    public abstract class Activation2dLayer<TInput, TOutput> : IScheduleLayer
        where TInput : struct
        where TOutput : struct
    {
        public readonly NativeArray<TInput> Input;
        public readonly NativeArray<TOutput> Output;

        public readonly ActivationFunction Function;

        public JobHandle Handle { get; protected set; }

        public Activation2dLayer(NativeArray<TInput> input, NativeArray<TOutput> output,
            ActivationFunction activation)
        {
            Input = input;
            Output = new NativeArray<TOutput>(input.Length, Allocator.Persistent);
            Function = activation;
            
            Handle = new JobHandle();
            Handle.Complete();
        }

        public abstract JobHandle Schedule();
    }
}