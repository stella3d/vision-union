using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion
{
    [BurstCompile]
    public struct ImageNormalize01Job : IJobParallelFor
    {
        public NativeArray<float> Data;

        public float2 MinMax;

        readonly float floorDiff;
        readonly float multiplyFactor;

        public ImageNormalize01Job(NativeArray<float> data, float2 minMax)
        {
            Data = data;
            MinMax = minMax;

            var minimum = MinMax.x;
            var maximum = MinMax.y;
            var rangeMagnitude = maximum - minimum;
            floorDiff = 0 - minimum;
            multiplyFactor = 1f / rangeMagnitude;
        }

        public void Execute(int index)
        {
            Data[index] = (Data[index] + floorDiff) * multiplyFactor;
        }
    }
}
