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
        
        [ReadOnly] public ImageData<T> Input;
        [WriteOnly] public ImageData<T> Output;

        public ImagePadJob(ImageData<T> input, ImageData<T> output, Padding padding, T value = default(T))
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