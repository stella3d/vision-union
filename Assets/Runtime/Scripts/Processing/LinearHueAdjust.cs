using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public struct LinearHueAdjustInPlaceFloat3 : IJobParallelFor
{
    public float3 Weights;
    public NativeArray<float3> Image;

    public void Execute(int index)
    {
        Image[index] += Weights;
    }
}

public struct LinearHueAdjustFloat3 : IJobParallelFor
{
    public float3 Weights;
    public NativeArray<float3> Image;
    public NativeArray<float3> Output;

    public void Execute(int index)
    {
        Output[index] = Image[index] + Weights;
    }
}
