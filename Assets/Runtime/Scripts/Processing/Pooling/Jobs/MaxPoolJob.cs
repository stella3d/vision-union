using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct MaxPoolJob<T> : IJob
        where T: struct
    {
        public Vector2Int Size;
        public Vector2Int Stride;
        
        [ReadOnly] public ImageData<T> Input;
        [WriteOnly] public ImageData<T> Output;

        public MaxPoolJob(ImageData<T> input, ImageData<T> output, Vector2Int size, Vector2Int stride)
        {
            Input = input;
            Output = output;
            Size = size;
            Stride = stride;
        }

        public void Execute()
        {
            //Pool.Max(Input, Output, Size, Stride);
            // TODO - make Pool() API friendlier to new structs
        }
    }
}