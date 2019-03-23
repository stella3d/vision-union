using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    // TODO - better names for these jobs!
    public struct FloatWithFloatConvolveJob : IJob, IDisposable
    {
        [ReadOnly] public Convolution2D<float> Convolution;
        [ReadOnly] public Image<float> Input;
        [WriteOnly] public Image<float> Output;

        public FloatWithFloatConvolveJob(Convolution2D<float> convolution, 
            Image<float> input, Image<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }
        
        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }

        public void Dispose()
        {
            Convolution.Dispose();
            Input.Dispose();
            Output.Dispose();
        }
    }
    
    [BurstCompile]
    public struct FloatWithFloat3ConvolveJob : IJob, IDisposable
    {
        [ReadOnly] public Convolution2D<float> Convolution;
        [ReadOnly] public Image<float3> Input;
        [WriteOnly] public Image<float3> Output;

        public FloatWithFloat3ConvolveJob(Convolution2D<float> convolution, 
            Image<float3> input, Image<float3> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }
        
        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }

        public void Dispose()
        {
            Convolution.Dispose();
            Input.Dispose();
            Output.Dispose();
        }
    }
    
    [BurstCompile]
    public struct FloatWithFloat3VectorConvolveJob : IJob, IDisposable
    {
        [ReadOnly] public Convolution2D<float> Convolution;
        [ReadOnly] public Image<float3> Input;
        [WriteOnly] public Image<float3> Output;

        public FloatWithFloat3VectorConvolveJob(Convolution2D<float> convolution, 
            Image<float3> input, Image<float3> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }
        
        public void Execute()
        {
            Convolution.ConvolveVector(Input, Output);
        }

        public void Dispose()
        {
            Convolution.Dispose();
            Input.Dispose();
            Output.Dispose();
        }
    }
    
    [BurstCompile]
    public struct Float2WithFloat3ConvolveJob : IJob, IDisposable
    {
        [ReadOnly] public Convolution2D<float2> Convolution;
        [ReadOnly] public Image<float3> Input;
        [WriteOnly] public Image<float3> Output;

        public Float2WithFloat3ConvolveJob(Convolution2D<float2> convolution, 
            Image<float3> input, Image<float3> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }
        
        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }

        public void Dispose()
        {
            Convolution.Dispose();
            Input.Dispose();
            Output.Dispose();
        }
    }
}