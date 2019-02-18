using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct ImagePadJob<T> : IJob
        where T: struct
    {
        public T Value;
        public Padding Padding;
        
        [ReadOnly] public Image<T> Input;
        [WriteOnly] public Image<T> Output;

        public ImagePadJob(Image<T> input, Image<T> output, Padding padding, T value = default(T))
        {
            Input = input;
            Output = output;
            Padding = padding;
            Value = value;
        }
        
        public void Execute()
        {
            Pad.Constant(Input, Output, Padding, Value);
        }
    }
}