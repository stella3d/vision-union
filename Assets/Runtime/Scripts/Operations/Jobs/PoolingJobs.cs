using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace BurstVision
{
    [BurstCompile]
    public struct MaxPool2x2Job : IJob
    {
        [ReadOnly] public Kernel<short> Kernel;
        [ReadOnly] public NativeArray<byte> Input;
        [WriteOnly] public NativeArray<float> Output;
        
        public int Height;
        public int Width;

        public MaxPool2x2Job(Kernel<short> kernel, NativeArray<byte> input, NativeArray<float> output,
            int width, int height, int xStride = 1, int yStride = 1)
        {
            Kernel = kernel;
            Input = input;
            Output = output;
            Width = width;
            Height = height;
        }

        public void Execute()
        {
            Kernel.Convolve(Input, Output, Width, Height);
        }
    }
}