using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct Float3ConvolveJob : IJob
    {
        [ReadOnly] public Convolution2D<float3> Convolution;
        [ReadOnly] public ImageData<float3> Input;
        [WriteOnly] public ImageData<float3> Output;

        public Float3ConvolveJob(Convolution2D<float3> convolution, 
            ImageData<float3> input, ImageData<float3> output)
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