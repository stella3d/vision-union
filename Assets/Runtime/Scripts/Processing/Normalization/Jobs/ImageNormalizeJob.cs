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

        public NativeArray<float> MinMax;

        readonly float floorDiff;
        readonly float multiplyFactor;

        public ImageNormalize01Job(NativeArray<float> data, NativeArray<float> minMax)
        {
            Data = data;
            MinMax = minMax;

            var minimum = MinMax[0];
            var maximum = MinMax[1];
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
