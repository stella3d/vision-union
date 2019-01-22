using System;
using Unity.Collections;
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

        public ImageData<TData> InputImage;
        
        static readonly NativeList<JobHandle> k_ParallelHandles = 
            new NativeList<JobHandle>(16, Allocator.Persistent);
        
        protected ParallelConvolutionJobSequence(ImageData<TData> input, 
            ParallelConvolutionSequences<TData> convolutions, 
            ParallelJobSequences<TJob> jobs)
        {
            InputImage = input;
            Convolutions = convolutions;
            Jobs = jobs;

            if (convolutions.Width != jobs.Width)
            {
                Debug.LogWarningFormat("convolutions ({0}) & jobs ({1}) must  be same length !",
                    convolutions.Width, jobs.Width);
            }

            var pad = convolutions[0, 0].Padding;

            Images = new ImageData<TData>[jobs.Width];
            InitializeImageData(input.Width - pad.x * 2, input.Height - pad.y * 2);
            
            InitializeJobs();
        }

        public void InitializeImageData(int width, int height)
        {
            for (var i = 0; i < Images.Length; i++)
            {
                Images[i] = new ImageData<TData>(width, height);
            }
        }
        
        public JobHandle Schedule(JobHandle dependency)
        {
            k_ParallelHandles.Clear();
            var handle = dependency;
            foreach (JobSequence<TJob> jobSequence in Jobs)
            {
                k_ParallelHandles.Add(jobSequence.Schedule(handle));
            }

            handle = JobHandle.CombineDependencies(k_ParallelHandles);
            return handle;
        }

        internal void ForEachSequence(Action<ConvolutionSequence<TData>, JobSequence<TJob>, ImageData<TData>> action)
        {
            for (var i = 0; i < Convolutions.Width; i++)
            {
                var image = Images[i];
                var jobSequence = Jobs[i];
                var convSequence = Convolutions[i];
                
                action(convSequence, jobSequence, image);
            }
        }

        /// <summary>
        /// Inside the implementation:
        /// 1) create all the job structs
        /// 2) assign the same input image to every job
        /// 3) assign an output image to every job - one image per sequence
        /// </summary>
        public abstract void InitializeJobs();

        public void Dispose()
        {
            Convolutions.Dispose();
            Images.Dispose();
        }
    }
}

