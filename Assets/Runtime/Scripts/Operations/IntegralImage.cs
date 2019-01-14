using System;
using System.Runtime.InteropServices;
using BurstVision;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct IntegralImageFromGrayscaleJob : IJob
{
    public int width;
    public int height;
    
    [ReadOnly]
    public NativeArray<float> GrayscaleTexture;
    
    public NativeArray<float> IntegralTexture;

    public void Execute()
    {
        var width = this.width;
        
        // set the top left pixel by itself so we don't have to branch during iteration.
        // since this is the first pixel, the output and input are the same
        IntegralTexture[0] = GrayscaleTexture[0];

        // do the rest of the top row 
        float previousSum = 0f;
        for (int w = 1; w < width; w++)
        {
            var localIntensity = GrayscaleTexture[w];
            var summedIntensity = localIntensity + previousSum;
            previousSum = summedIntensity;
            IntegralTexture[w] = summedIntensity;
        }
        
        for (int h = 1; h < height; h++)
        {
            var yIndex = h * width;
            var firstLocalIntensity = GrayscaleTexture[yIndex];
            
            var firstTopIndex = (h - 1) * width;
            var firstTopSumIntensity = IntegralTexture[firstTopIndex];

            var firstSum = firstLocalIntensity + firstTopSumIntensity;
            IntegralTexture[yIndex] = firstSum;
            
            for (int w = 1; w < width; w++)
            {
                var index = yIndex + w;
                
                var localIntensity = GrayscaleTexture[index];

                var leftIndex = index - 1;
                var topIndex = firstTopIndex + w;
                var topLeftIndex = topIndex - 1;

                var leftSumIntensity = IntegralTexture[leftIndex];
                var topSumIntensity = IntegralTexture[topIndex];
                var topLeftSumIntensity = IntegralTexture[topLeftIndex];

                var summedIntensity = localIntensity + leftSumIntensity + topSumIntensity - topLeftSumIntensity;
                IntegralTexture[index] = summedIntensity;
            }
        }
    }
}

[BurstCompile]
public struct IntegralImageFromGrayscaleByteJob : IJob
{
    public int width;
    public int height;
    
    [ReadOnly]
    public NativeArray<byte> GrayscaleTexture;
    
    public NativeArray<int> IntegralTexture;

    public void Execute()
    {
        var width = this.width;
        
        // set the top left pixel by itself so we don't have to branch during iteration.
        // since this is the first pixel, the output and input are the same
        IntegralTexture[0] = GrayscaleTexture[0];

        // do the rest of the top row 
        var previousSum = 0;
        for (int w = 1; w < width; w++)
        {
            var localIntensity = GrayscaleTexture[w];
            var summedIntensity = localIntensity + previousSum;
            previousSum = summedIntensity;
            IntegralTexture[w] = summedIntensity;
        }
        
        for (int h = 1; h < height; h++)
        {
            var yIndex = h * width;
            var firstLocalIntensity = GrayscaleTexture[yIndex];
            
            var firstTopIndex = yIndex - width;
            var firstTopSumIntensity = IntegralTexture[firstTopIndex];

            var firstSum = firstLocalIntensity + firstTopSumIntensity;
            IntegralTexture[yIndex] = firstSum;
            
            for (int w = 1; w < width; w++)
            {
                var index = yIndex + w;
                
                var localIntensity = GrayscaleTexture[index];

                var leftIndex = index - 1;
                var topIndex = firstTopIndex + w;
                var topLeftIndex = topIndex - 1;

                var leftSumIntensity = IntegralTexture[leftIndex];
                var topSumIntensity = IntegralTexture[topIndex];
                var topLeftSumIntensity = IntegralTexture[topLeftIndex];

                var summedIntensity = localIntensity + leftSumIntensity + topSumIntensity - topLeftSumIntensity;
                IntegralTexture[index] = summedIntensity;
            }
        }
    }
}

[BurstCompile]
public struct GrayscaleFromColor24Job : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<Color24> InputTexture;
    
    [WriteOnly]
    public NativeArray<float> Grayscale;

    public void Execute()
    {
        for (int index = 0; index < InputTexture.Length; index++)
        {
            var p = InputTexture[index];
            const float oneOver765 = 0.00130718954f;
            Grayscale[index] = (p.r + p.g + p.b) * oneOver765 * 3;
        }
    }

    public void Execute(int index)
    {
        var p = InputTexture[index];
        const float oneOver765 = 0.00130718954f;
        Grayscale[index] = (p.r + p.g + p.b) * oneOver765 * 3;
    }
}


[BurstCompile]
public struct Grayscale8FromColor24Job : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<Color24> InputTexture;
    
    [WriteOnly]
    public NativeArray<byte> Grayscale;

    public void Execute(int index)
    {
        var p = InputTexture[index];
        // use the luminosity 
        Grayscale[index] = Convert.ToByte((p.r * 0.21f + p.g * 0.72f + p.b * 0.07f));
    }
}

[BurstCompile]
public struct AverageIntensity3x3IntJob : IJob
{
    public int width;
    public int height;
    
    [ReadOnly]
    public NativeArray<int> Integral;
    
    [WriteOnly]
    public NativeArray<float> Intensities;

    // ((x + 1, y + 1) - (x + 1, 0)) - ((0, y) - (0, 0))
    public void Execute()
    {
        Operations.Average3x3(Integral, Intensities, width, height);
    }
}

[BurstCompile]
public struct MaxPool2x2GrayscaleJob : IJob
{
    public int width;
    public int height;
    
    [ReadOnly]
    public NativeArray<byte> Input;
    
    [WriteOnly]
    public NativeArray<byte> Output;

    // ((x + 1, y + 1) - (x + 1, 0)) - ((0, y) - (0, 0))
    public void Execute()
    {
        Operations.MaxPool2x2(Input, Output, width, height);
    }
}

[BurstCompile]
public struct MeanPool2x2GrayscaleJob : IJob
{
    public int width;
    public int height;
    
    [ReadOnly]
    public NativeArray<byte> Input;
    
    [WriteOnly]
    public NativeArray<float> Output;

    // ((x + 1, y + 1) - (x + 1, 0)) - ((0, y) - (0, 0))
    public void Execute()
    {
        Operations.MeanPool2x2(Input, Output, width, height);
    }
}

[BurstCompile]
public struct SobelJob: IJob
{
    public int width;
    public int height;

    public float threshold;
    
    [ReadOnly]
    public NativeArray<byte> Grayscale;
    
    [WriteOnly]
    public NativeArray<byte> SobelOut;
    
    public void Execute()
    {
        Operations.Sobel(Grayscale, SobelOut, threshold, width, height);
    }
}
