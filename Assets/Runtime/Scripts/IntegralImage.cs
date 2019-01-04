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
public struct GrayscaleFromColor24Job : IJob, IJobParallelFor
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
            Grayscale[index] = (p.r + p.g + p.b) * oneOver765;
        }
    }

    public void Execute(int index)
    {
        var p = InputTexture[index];
        const float oneOver765 = 0.00130718954f;
        Grayscale[index] = (p.r + p.g + p.b) * oneOver765;
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
        Grayscale[index] = Convert.ToByte((p.r * 0.21f + p.g * 0.72f + p.b * 0.07f) / 3);
    }
}

[BurstCompile]
public struct AverageIntensity3x3Job : IJob
{
    public int width;
    public int height;
    
    [ReadOnly]
    public NativeArray<float> Integral;
    
    [WriteOnly]
    public NativeArray<float> Intensities;

    // ((x + 1, y + 1) - (x + 1, 0)) - ((0, y) - (0, 0))
    public void Execute()
    {
        var zeroPixel = Integral[0];
        for (int i = 1; i < height - 1; i++)
        {
            var rowIndex = i * width;
            var bottomYIndex = rowIndex + width;
            var bottomYIndexPlusOne = bottomYIndex + 1;
            
            var areaBottomRowStart = Integral[bottomYIndex];
            
            for (int n = 1; n < width - 1; n++)
            {
                var areaRightColumnStart = Integral[n];
                var areaBottomRight = Integral[bottomYIndexPlusOne + n];

                var intensitySum = areaBottomRight - areaRightColumnStart - areaBottomRowStart - zeroPixel;
                Intensities[rowIndex + n] = intensitySum / 9;
            }
        }
    }
}

[BurstCompile]
// TODO - does this actually work ?
public struct AverageIntensity3x3IntJob : IJob
{
    public int width;
    public int height;
    
    [ReadOnly]
    public NativeArray<int> Integral;
    
    [WriteOnly]
    public NativeArray<int> Intensities;

    // ((x + 1, y + 1) - (x + 1, 0)) - ((0, y) - (0, 0))
    public void Execute()
    {
        var zeroPixel = Integral[0];
        for (int i = 1; i < height - 1; i++)
        {
            var rowIndex = i * width;
            var bottomYIndex = rowIndex + width;
            var bottomYIndexPlusOne = bottomYIndex + 1;
            
            var areaBottomRowStart = Integral[bottomYIndex];
            
            for (int n = 1; n < width - 1; n++)
            {
                var areaRightColumnStart = Integral[n];
                var areaBottomRight = Integral[bottomYIndexPlusOne + n];

                var intensitySum = areaBottomRight - areaRightColumnStart - areaBottomRowStart - zeroPixel;
                Intensities[rowIndex + n] = intensitySum / 9;
            }
        }
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

[StructLayout(LayoutKind.Sequential)]
public struct Color24
{
    public byte r;
    public byte g;
    public byte b;

    public Color24(byte r, byte g, byte b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }
}
