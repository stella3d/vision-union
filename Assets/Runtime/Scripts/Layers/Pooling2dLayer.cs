using Unity.Collections;
using Unity.Jobs;

namespace BurstVision
{
    public abstract class Pooling2dLayer<TInput, TOutput>
        where TInput : struct
        where TOutput : struct
    {
        public readonly NativeArray<TInput> Input;
        public readonly NativeArray<TOutput> Output;

        public readonly PoolingOptions Pooling;

        public JobHandle Handle { get; protected set; }

        public Pooling2dLayer(NativeArray<TInput> input, NativeArray<TOutput> output,
            PoolingOptions pooling)
        {
            Input = input;
            Output = new NativeArray<TOutput>(input.Length, Allocator.Persistent);
            Pooling = pooling;
            Handle = new JobHandle();
            Handle.Complete();
        }

        public abstract void Schedule();
    }
}