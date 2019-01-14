using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace BurstVision
{
    public abstract class Convolution2dLayer<TKernel, TOutput> : IScheduleLayer
        where TKernel : struct
        where TOutput : struct
    {
        public Texture2D Input;
        public NativeArray<TOutput>[] Output;

        public readonly KernelSet<TKernel> FilterKernels;

        public readonly Vector2Int KernelSize;
        public readonly Vector2Int Strides;
        
        public JobHandle Handle { get; protected set; }

        public Convolution2dLayer(Texture2D input,
            Kernel<TKernel>[,] kernels,
            NativeArray<TOutput>[] output,
            Vector2Int kernelSize,
            Vector2Int strides)
        {
            Input = input;
            Output = output;

            FilterKernels = new KernelSet<TKernel>(kernels);
            KernelSize = kernelSize;
            Strides = strides;
            
            Handle = new JobHandle();
            Handle.Complete();
        }

        public abstract JobHandle Schedule();
    }

    public class ShortConvolution2dLayer<TOutput> : Convolution2dLayer<short, TOutput>
        where TOutput : struct
    {
        public ShortConvolution2dLayer(Texture2D inputTexture,
            Kernel<short>[,] kernels,
            NativeArray<TOutput>[] output,
            Vector2Int strides,
            Vector2Int kernelSize)
            :
                base(inputTexture, kernels, output, kernelSize, strides)
        {
        }

        public override JobHandle Schedule()
        {
            // schedule filter kernel jobs for each channel
            
            // schedule pooling per channel
            
            // schedule the channel combine job if channels > 1
            
            // 

            return default(JobHandle);
        }
    }

    public class FloatConvolution2dLayer<TOutput> : Convolution2dLayer<float, TOutput>
        where TOutput : struct
    {
        public FloatConvolution2dLayer(Texture2D inputTexture,
            Kernel<float>[,] kernels,
            NativeArray<TOutput>[] output,
            Vector2Int strides,
            Vector2Int kernelSize)
            :
                base(inputTexture, kernels, output, kernelSize, strides)
        {
        }
        
        public override JobHandle Schedule()
        {
            return default(JobHandle);
        }
    }
}