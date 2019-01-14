using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

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
    
    
    public static partial class KernelMethods
    {
        public static void Convolve(this Kernel<short> kernel, 
            NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut,
            int width, int height, 
            int xStride = 1, int yStride = 1)
        {
            var xPad = (kernel.Width - 1) / 2;
            var yPad = (kernel.Height - 1) / 2;
            
            for (var r = yPad; r < height - yPad; r += yStride)
            {
                var rowIndex = r * width;
                for (var c = xPad; c < width - xPad; c += xStride)
                {
                    var centerPixelIndex = rowIndex + c;
                    var kernelSum = kernel.Accumulate(pixelBuffer, centerPixelIndex, width, xPad, yPad);

                    kernelSum /= 255;
                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
        
        public static void Convolve(this Kernel<float> kernel, 
            NativeArray<byte> pixelBuffer, NativeArray<float> pixelOut,
            int width, int height, 
            uint xStride = 1, uint yStride = 1)
        {
            var xPad = (kernel.Width - 1) / 2;
            var yPad = (kernel.Height - 1) / 2;
            
            for (var r = yPad; r < height - yPad; r++)
            {
                var rowIndex = r * width;
                for (var c = xPad; c < width - xPad; c++)
                {
                    var centerPixelIndex = rowIndex + c;
                    // TODO make padding and kernel offsets different values
                    var kernelSum = kernel.Accumulate(pixelBuffer, centerPixelIndex, width, xPad, yPad);

                    pixelOut[centerPixelIndex] = kernelSum;
                }
            }
        }
    }
}