using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace VisionUnion.Organization
{
    /// <summary>
    /// Defines a group of convolutions and how to schedule their jobs.
    /// Allows scheduling all component jobs as one.
    /// </summary>
    /// <typeparam name="TData">The data type of the convolution and image</typeparam>
    /// <typeparam name="TJob">The concrete type of job that will use this data</typeparam>
    public abstract class ParallelConvolutionJobSequence<TData, TJob> : IDisposable
        where TData: struct
        where TJob: struct, IJob
    {
        /// <summary>
        /// The definition of the set of convolution sequences to run
        /// </summary>
        public readonly ParallelConvolutionSequences<TData> Convolutions;

        /// <summary>
        /// The jobs that will execute the convolution definitions
        /// </summary>
        public readonly ParallelJobSequences<TJob> Jobs;

        /// <summary>
        /// The image outputs of each sequence
        /// </summary>
        public readonly ImageData<TData>[] Images;

        public ImageData<TData> InputImage;
        
        static readonly NativeList<JobHandle> k_ParallelHandles = 
            new NativeList<JobHandle>(16, Allocator.Persistent);
        
        protected ParallelConvolutionJobSequence(ImageData<TData> input, 
            ParallelConvolutionSequences<TData> convolutions, 
            ParallelJobSequences<TJob> jobs)
        {
            if (convolutions.Width != jobs.Width)
            {
                Debug.LogWarningFormat("convolutions ({0}) & jobs ({1}) must  be same length !",
                    convolutions.Width, jobs.Width);

                return;
            }
            
            InputImage = input;
            Convolutions = convolutions;
            Jobs = jobs;

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

        public void ForEachSequence(Action<ConvolutionSequence<TData>, JobSequence<TJob>, ImageData<TData>> action)
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
        /// As well as whatever else is needed for that job type
        /// </summary>
        public abstract void InitializeJobs();

        public void Dispose()
        {
            Convolutions.Dispose();
            Images.Dispose();
        }
    }
}

