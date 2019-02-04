using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    public struct ImageSubtractionJob : IJobParallelFor
    {
        [ReadOnly] public ImageData<float> Input1;
        [ReadOnly] public ImageData<float> Input2;
        [WriteOnly] public ImageData<float> Output;

        public void Execute(int index)
        {
            var one = Input1.Buffer[index];
            var two = Input2.Buffer[index];
            Output.Buffer[index] = one - two;
        }
    }
}

