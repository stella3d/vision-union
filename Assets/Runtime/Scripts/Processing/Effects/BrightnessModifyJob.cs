using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion.Jobs
{
    public struct BrightnessModifyJob : IJobParallelFor
    {
        public float Modifier;
        
        [ReadOnly] public ImageData<float> Input;
        [WriteOnly] public ImageData<float> Output;

        public void Execute(int index)
        {
            Output.Buffer[index] = math.clamp(Input.Buffer[index] + Modifier, 0f, 1f);
        }
    }
}

