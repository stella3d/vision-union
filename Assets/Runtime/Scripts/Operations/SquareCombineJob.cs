using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct SquareCombineJob : IJobParallelFor
    {
        [ReadOnly] public ImageData<float> A;
        [ReadOnly] public ImageData<float> B;
        [WriteOnly] public ImageData<float> Output;

        public void Execute(int index)
        {
            var x = A.Buffer[index];
            var y = B.Buffer[index];
            Output.Buffer[index] = math.sqrt(x * x + y * y);
        }
    }
}