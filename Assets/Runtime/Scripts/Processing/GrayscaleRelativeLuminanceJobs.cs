using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

// Luma vs "relative luminance" and their RGB->greyscale conversions
// are covered at:  https://en.wikipedia.org/wiki/Luma_(video)
namespace VisionUnion.Jobs
{
    // default weights for relative luminance calculation
    static class LuminanceWeights
    {
        public const float red = 0.2126f;
        public const float green = 0.7152f;
        public const float blue = 0.0722f;
    }

    [BurstCompile]
    public struct GreyscaleByLuminanceFloatJob : IJobParallelFor
    {
        public Color24 Weights;
        
        [ReadOnly] public NativeArray<Color48> InputTexture;
    
        [WriteOnly] public NativeArray<float> Grayscale;

        public void Execute(int index)
        {
            var p = InputTexture[index];
            Grayscale[index] = p.r * Weights.r + p.g * Weights.g + p.b * Weights.b;
        }
    }

    [BurstCompile]
    public struct GreyscaleLuminanceByteJob : IJobParallelFor
    {
        public Color24 Weights;
        
        [ReadOnly] public NativeArray<Color24> InputTexture;
    
        [WriteOnly] public NativeArray<byte> Grayscale;

        public void Execute(int index)
        {
            var p = InputTexture[index];
            Grayscale[index] = Convert.ToByte(p.r * Weights.r + p.g * Weights.g + p.b * Weights.b);
        }
    }
}