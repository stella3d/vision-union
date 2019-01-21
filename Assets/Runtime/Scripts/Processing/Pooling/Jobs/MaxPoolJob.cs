using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace VisionUnion.Jobs
{
    //[BurstCompile]
    public struct MaxPoolByteJob : IJob
    {
        public Vector2Int Size;
        public Vector2Int Stride;
        
        [ReadOnly] public ImageData<byte> Input;
        [WriteOnly] public ImageData<byte> Output;

        public MaxPoolByteJob(ImageData<byte> input, ImageData<byte> output, Vector2Int size, Vector2Int stride)
        {
            Input = input;
            Output = output;
            Size = size;
            Stride = stride;
        }

        public void Execute()
        {
            Pool.Max(Input, Output, Size, Stride);
        }
    }
}