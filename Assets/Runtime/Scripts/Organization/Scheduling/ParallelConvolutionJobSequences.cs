using System;
using Unity.Jobs;
using UnityEngine;

namespace VisionUnion.Organization
{
    public abstract class ParallelConvolutionJobSequence<TData, TJob> : IDisposable
        where TData: struct
        where TJob: struct, IJob
    {
        public readonly ParallelConvolutionSequences<TData> Convolutions;

        public readonly ParallelJobSequences<TJob> Jobs;

        public readonly ImageData<TData>[] Images;
        
        protected ParallelConvolutionJobSequence(int width, int height,
            ParallelConvolutionSequences<TData> convolutions, 
            ParallelJobSequences<TJob> jobs)
        {
            Convolutions = convolutions;
            Jobs = jobs;

            if (convolutions.Width != jobs.Width)
            {
                Debug.LogWarningFormat("convolutions ({0}) & jobs ({1}) must  be same length !",
                    convolutions.Width, jobs.Width);
            }

            Images = new ImageData<TData>[jobs.Width];
            InitializeImageData(width, height);
        }

        public void InitializeImageData(int width, int height)
        {
            for (var i = 0; i < Images.Length; i++)
            {
                Images[i] = new ImageData<TData>(width, height);
            }
        }

        public void Dispose()
        {
            Convolutions.Dispose();
            Images.Dispose();
        }
    }
}

