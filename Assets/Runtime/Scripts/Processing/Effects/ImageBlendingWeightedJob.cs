using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    public struct ImageBlendingWeightedJob : IJobParallelFor
    {
        public float Weight1;
        public float Weight2;
        
        [ReadOnly] public ImageData<float> Input1;
        [ReadOnly] public ImageData<float> Input2;
        [WriteOnly] public ImageData<float> Output;

        public void Execute(int index)
        {
            var one = Input1.Buffer[index] * Weight1;
            var two = Input2.Buffer[index] * Weight2;
            Output.Buffer[index] = one + two;
        }
    }
}

