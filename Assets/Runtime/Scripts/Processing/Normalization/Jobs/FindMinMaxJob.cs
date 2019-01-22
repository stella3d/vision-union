using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct FindMinMaxJob : IJob
    {
        [ReadOnly] public NativeArray<float> Data;

        public float2 MinMaxOutput;

        public FindMinMaxJob(NativeArray<float> data)
        {
            Data = data;
            MinMaxOutput = new int2();
        }

        public void Execute()
        {
            MinMaxOutput = Data.FindMinMax();
        }
    }
}
