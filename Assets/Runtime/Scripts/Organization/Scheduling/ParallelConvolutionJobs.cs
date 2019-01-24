﻿using System;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Organization
{
    /// <summary>
    /// Defines a group of convolutions and how to schedule their jobs.
    /// Allows scheduling all component jobs as one.
    /// </summary>
    /// <typeparam name="TData">The data type of the convolution and image</typeparam>
    /// <typeparam name="TJob">The concrete type of job that will use this data</typeparam>
    public abstract class ParallelConvolutionJobs<TData, TJob> : IDisposable
        where TData: struct
        where TJob: struct, IJob
    {
        // TODO - split out or merge the convolution + image data with the jobs, since the jobs duplicate all that
        
        /// <summary>
        /// The definition of the set of convolution sequences to run
        /// </summary>
        public readonly ParallelConvolutionSequences<TData>[] Convolutions;

        /// <summary>
        /// The jobs that will execute the convolution definitions
        /// </summary>
        public readonly ParallelJobSequences<TJob>[] Jobs;

        /// <summary>
        /// The image outputs of each sequence
        /// </summary>
        public readonly ImageData<TData>[][] Images;

        public ImageData<TData> InputImage;

        const int k_MaxSequences = 32;
        protected readonly NativeList<JobHandle> m_ParallelHandles = 
            new NativeList<JobHandle>(k_MaxSequences, Allocator.Persistent);
        
        protected ParallelConvolutionJobs(ImageData<TData> input, 
            ParallelConvolutionSequences<TData> convolutions)
        {
            InputImage = input;
            Convolutions = new [] {convolutions};
            Jobs = new ParallelJobSequences<TJob>[3];

            var pad = Convolutions[0][0, 0].Padding;

            Images = new ImageData<TData>[Jobs[0].Width][];
            InitializeImageData(input.Width - pad.x * 2, input.Height - pad.y * 2);
            
            InitializeJobs();
        }
        
        protected ParallelConvolutionJobs(ImageData<TData> input, 
            ParallelConvolutionSequences<TData>[] convolutions)
        {
            InputImage = input;
            Convolutions = convolutions;
            Jobs = new ParallelJobSequences<TJob>[3];

            var pad = convolutions[0][0, 0].Padding;

            Images = new ImageData<TData>[Jobs[0].Width][];
            InitializeImageData(input.Width - pad.x * 2, input.Height - pad.y * 2);
            
            InitializeJobs();
        }

        public JobHandle Schedule(JobHandle dependency)
        {
            m_ParallelHandles.Clear();
            var handle = dependency;
            foreach (var channel in Jobs)
            {
                foreach (JobSequence<TJob> jobSequence in channel)
                {
                    m_ParallelHandles.Add(jobSequence.Schedule(handle));
                }
            }

            handle = JobHandle.CombineDependencies(m_ParallelHandles);
            return handle;
        }
        
        public void InitializeImageData(int width, int height)
        {
            for (var n = 0; n < Jobs.Length; n++)
            {
                var channel = Jobs[n];
                for (var i = 0; i < Images.Length; i++)
                {
                    var existing = Images[n][i];
                    if (existing != default(ImageData<TData>) && existing.Buffer.IsCreated)
                        existing.Dispose();

                    Images[n][i] = new ImageData<TData>(width, height);
                }
            }
        }
        
        public void Dispose()
        {
            m_ParallelHandles.Dispose();
            //Convolutions.Dispose();
            Images[0].Dispose();
        }
        
        /// <summary>
        /// Inside the implementation:
        /// 1) create all the job structs
        /// 2) assign the same input image to every job
        /// 3) assign an output image to every job - one image per sequence
        /// As well as whatever else is needed for that job type
        /// </summary>
        public abstract void InitializeJobs();
    }
}
