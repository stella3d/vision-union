using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion.Jobs
{
    //[BurstCompile]
    public struct FindMinMaxJob : IJob
    {
        [ReadOnly] public NativeArray<float> Data;

        public NativeArray<float> MinMaxOutput;

        public FindMinMaxJob(NativeArray<float> data)
        {
            Data = data;
            MinMaxOutput = new NativeArray<float>(2, Allocator.TempJob);
        }

        public void Execute()
        {
            var minimum = float.MaxValue;
            var maximum = float.MinValue;
            for (var i = 0; i < Data.Length; i++)
            {
                var inputValue = Data[i];
                minimum = math.select(inputValue, minimum, inputValue > minimum);
                maximum = math.select(inputValue, maximum, inputValue < maximum);
            }

            MinMaxOutput[0] = minimum;
            MinMaxOutput[1] = maximum;
        }
    }
}
