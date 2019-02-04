using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct Float3x4ConvolveJob : IJob
    {
        [ReadOnly] public Convolution2D<float3x4> Convolution;
        [ReadOnly] public ImageData<float3x4> Input;
        [WriteOnly] public ImageData<float3x4> Output;

        public Float3x4ConvolveJob(Convolution2D<float3x4> convolution, 
            ImageData<float3x4> input, ImageData<float3x4> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }
        
        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
}