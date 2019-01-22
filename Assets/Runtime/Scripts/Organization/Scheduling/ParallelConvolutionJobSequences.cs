using System;
using Unity.Jobs;
using UnityEngine;

namespace VisionUnion.Organization
{
    public abstract class ParallelConvolutionJobSequence<TData, TJob> : IDisposable
        where TData: struct
        where TJob: struct, IJob
    {
        public readonly ParallelConvolutionSequences<TData> ConvolutionSequences;

        public readonly ParallelJobSequences<TJob> JobSequences;

        public readonly ImageData<TData>[] Images;
        
        protected ParallelConvolutionJobSequence(int width, int height,
            ParallelConvolutionSequences<TData> convolutionSequences, 
            ParallelJobSequences<TJob> jobSequences)
        {
            ConvolutionSequences = convolutionSequences;
            JobSequences = jobSequences;

            if (convolutionSequences.Width != jobSequences.Width)
            {
                Debug.LogWarningFormat("convolutions ({0}) & jobs ({1}) must  be same length !",
                    convolutionSequences.Width, jobSequences.Width);
            }

            Images = new ImageData<TData>[jobSequences.Width];
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
            ConvolutionSequences.Dispose();
            Images.Dispose();
        }
    }
}

