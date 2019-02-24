using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

// Luma vs "relative luminance" and their RGB->greyscale conversions
// are covered at:  https://en.wikipedia.org/wiki/Luma_(video)
namespace VisionUnion.Jobs
{
    // default weights for relative luminance calculation
    public static class LuminanceWeights
    {
        public const float oneOver255 = 0.0039215686f;
        
        public static Color96 Float
        {
            get { return new Color96(0.2126f, 0.7152f, 0.0722f); }
        }
        
        public static Color96 FloatNormalized
        {
            get
            {
                return new Color96(0.2126f * oneOver255, 0.7152f * oneOver255, 0.0722f * oneOver255);
            }
        }

        public static Color24 Byte
        {
            get
            {
                var red = Convert.ToByte(Float.r * 255f);
                var green = Convert.ToByte(Float.g * 255f);
                var blue = Convert.ToByte(Float.b * 255f);
                return new Color24(red, green, blue);
            }
        }
    }
    
    [BurstCompile]
    public struct GreyscaleByLuminanceFloatJob : IJobParallelFor
    {
        public Color96 Weights;
        
        [ReadOnly] public NativeArray<Color96> InputTexture;
    
        [WriteOnly] public NativeArray<float> Grayscale;

        public GreyscaleByLuminanceFloatJob(NativeArray<Color96> input, 
            NativeArray<float> grayscale, 
            Color96 weights)
        {
            InputTexture = input;
            Grayscale = grayscale;
            Weights = weights;
            if (Weights.Equals(default(Color96)))
                Weights = LuminanceWeights.Float;
        }
        
        public void Execute(int index)
        {
            var p = InputTexture[index];
            Grayscale[index] = p.r * Weights.r + p.g * Weights.g + p.b * Weights.b;
        }
    }
    
    [BurstCompile]
    public struct GreyscaleByLuminanceFloatJob24 : IJobParallelFor
    {
        public Color96 Weights;
        
        [ReadOnly] public NativeArray<Color24> InputTexture;
    
        [WriteOnly] public NativeArray<float> Grayscale;

        public GreyscaleByLuminanceFloatJob24(NativeArray<Color24> input, 
            NativeArray<float> grayscale, 
            Color96 weights)
        {
            InputTexture = input;
            Grayscale = grayscale;
            if (weights.Equals(default(Color96)))
                weights = LuminanceWeights.FloatNormalized;

            Weights = weights;
        }
        
        public void Execute(int index)
        {
            var p = InputTexture[index];
            Grayscale[index] = p.r * Weights.r + p.g * Weights.g + p.b * Weights.b;
        }
    }
    
    [BurstCompile]
    public struct Color24ToFloat3Job : IJobParallelFor
    {
        public float3 Weights;
        
        [ReadOnly] public NativeArray<Color24> Input;
    
        [WriteOnly] public NativeArray<float3> Output;

        public Color24ToFloat3Job(NativeArray<Color24> input, NativeArray<float3> output, Color96 weights = default(Color96))
        {
            Input = input;
            Output = output;
            if (weights.Equals(default(Color96)))
                weights = LuminanceWeights.FloatNormalized;

            Weights = weights;
        }
        
        public void Execute(int index)
        {
            var p = Input[index];
            const float oneOver255 = 0.0039215686f;
            Output[index] = new float3(p.r * oneOver255, p.g * oneOver255, p.b * oneOver255);
        }
    }
    
    [BurstCompile]
    public struct Float3ToFloat4Job : IJobParallelFor
    {
        public float Alpha;
        
        [ReadOnly] public NativeArray<float3> Input;
    
        [WriteOnly] public NativeArray<float4> Output;

        public Float3ToFloat4Job(NativeArray<float3> input, NativeArray<float4> output, float alpha = 1f)
        {
            Input = input;
            Output = output;
            Alpha = alpha;
        }
        
        public void Execute(int index)
        {
            var p = Input[index];
            Output[index] = new float4(p.x, p.y, p.z, Alpha);
        }
    }

    [BurstCompile]
    public struct GreyscaleLuminanceByteJob : IJobParallelFor
    {
        public Color24 Weights;
        
        [ReadOnly] public NativeArray<Color24> InputTexture;
    
        [WriteOnly] public NativeArray<byte> Grayscale;
        
        public GreyscaleLuminanceByteJob(NativeArray<Color24> input, 
            NativeArray<byte> grayscale, 
            Color24 weights)
        {
            InputTexture = input;
            Grayscale = grayscale;
            Weights = weights;
            if (Weights.Equals(default(Color24)))
                Weights = LuminanceWeights.Byte;
        }

        public void Execute(int index)
        {
            var p = InputTexture[index];
            Grayscale[index] = Convert.ToByte(p.r * Weights.r + p.g * Weights.g + p.b * Weights.b);
        }
    }
    
    [BurstCompile]
    public struct RFloatToRgbByteJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float> GrayInput;
    
        [WriteOnly] public NativeArray<Color24> RgbOutput;

        public RFloatToRgbByteJob(NativeArray<float> input, NativeArray<Color24> output)
        {
            GrayInput = input;
            RgbOutput = output;
        }
        
        public void Execute(int index)
        {
            var r = (byte)(GrayInput[index] * byte.MaxValue);
            RgbOutput[index] = new Color24(r, r, r);
        }
    }
}