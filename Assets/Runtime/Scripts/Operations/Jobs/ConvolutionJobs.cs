using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace BurstVision
{
    [BurstCompile]
    public struct ShortKernelConvolveJob : IJob
    {
        [ReadOnly] public Kernel<short> Kernel;
        [ReadOnly] public NativeArray<byte> Input;
        [WriteOnly] public NativeArray<float> Output;
        
        public int Height;
        public int Width;

        public ShortKernelConvolveJob(Kernel<short> kernel, NativeArray<byte> input, NativeArray<float> output,
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
    
    [BurstCompile]
    public struct FloatKernelConvolveJob : IJob
    {
        [ReadOnly] public Kernel<float> Kernel;
        [ReadOnly] public NativeArray<byte> Input;
        [WriteOnly] public NativeArray<float> Output;
        
        public int Height;
        public int Width;

        public FloatKernelConvolveJob(Kernel<float> kernel, NativeArray<byte> input, NativeArray<float> output,
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