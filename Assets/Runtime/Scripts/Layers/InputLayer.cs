using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace BurstVision
{
    /// <summary>
    /// Use to transform input into the desired data 
    /// </summary>
    public abstract class InputLayer : IScheduleLayer
    {
        public readonly Texture2D Input;
        public readonly Texture2D Output;
        
        public JobHandle Handle { get; protected set; }

        public InputLayer(Texture2D input, Texture2D output)
        {
            Input = input;
            Output = output;
            Handle = new JobHandle();
            Handle.Complete();
        }

        public abstract JobHandle Schedule();
    }
}