using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    public struct MultiplyImageScalarJob : IJobParallelFor
    {
        public float Modifier;
        
        [ReadOnly] public ImageData<float> Input;
        [WriteOnly] public ImageData<float> Output;

        public void Execute(int index)
        {
            Output.Buffer[index] = Input.Buffer[index] * Modifier;
        }
    }
    
    public struct MultiplyImagesJob : IJobParallelFor
    {
        [ReadOnly] public ImageData<float> Input1;
        [ReadOnly] public ImageData<float> Input2;
        [WriteOnly] public ImageData<float> Output;

        public MultiplyImagesJob(ImageData<float> input1, ImageData<float> input2, ImageData<float> output)
        {
            Input1 = input1;
            Input2 = input2;
            Output = output;
        }

        public void Execute(int i)
        {
            Output.Buffer[i] = Input1.Buffer[i] * Input2.Buffer[i];
        }
    }
}

