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
        /// <summary>
        /// The jobs that will execute the convolution definitions
        /// </summary>
        public readonly ParallelJobSequences<TJob>[] Jobs;

        public readonly Image<TData>[] InputImages;

        const int k_MaxSequences = 32;
        protected readonly NativeList<JobHandle> m_ParallelHandles = 
            new NativeList<JobHandle>(k_MaxSequences, Allocator.Persistent);
        
        protected ParallelConvolutionJobs(ParallelConvolutionData<TData> data)
        {
            InputImages = data.InputImages;
            Jobs = new ParallelJobSequences<TJob>[data.channelCount];
        }
        
        public ParallelConvolutionJobs(ParallelConvolutionData<TData> data, JobHandle dependency, 
            Action<JobSequence<TJob>, ParallelConvolutionData<TData>.Sequence, int> action)
        {
            InputImages = data.InputImages;
            Jobs = new ParallelJobSequences<TJob>[data.channelCount];
            for (int i = 0; i < Jobs.Length; i++)
            {
                Jobs[i] = new ParallelJobSequences<TJob>(data.convolutionsPerChannel, 1);
            }

            for (int c = 0; c < data.Channels.Length; c++)
            {
                var jobs = Jobs[c];
                var inputImage = InputImages[c];
                for (int i = 0; i < data.convolutionsPerChannel; i++)
                {
                    var sequence = data[c, i];
                    var sequenceJobs = jobs[i];
                    action(sequenceJobs, sequence, i);
                }
            }
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
        
        // for scheduling each channel, without waiting for other channels in the image to complete.
        public void ScheduleSplit(JobHandle dependency, ref JobHandle[] handles)
        {
            var handle = dependency;
            for (var i = 0; i < Jobs.Length; i++)
            {
                var channel = Jobs[i];
                m_ParallelHandles.Clear();
                foreach (JobSequence<TJob> jobSequence in channel)
                {
                    m_ParallelHandles.Add(jobSequence.Schedule(handle));
                }
                
                handles[i] = JobHandle.CombineDependencies(m_ParallelHandles);
            }
        }
        
        // for scheduling each channel, without waiting for other channels in the image to complete,
        // with separate dependencies per channel.  can be used to chain channels
        public void ScheduleSplit(JobHandle[] dependencies, ref JobHandle[] handles)
        {
            for (var i = 0; i < Jobs.Length; i++)
            {
                var channel = Jobs[i];
                var dependency = dependencies[i];
                m_ParallelHandles.Clear();
                foreach (JobSequence<TJob> jobSequence in channel)
                {
                    m_ParallelHandles.Add(jobSequence.Schedule(dependency));
                }
                
                handles[i] = JobHandle.CombineDependencies(m_ParallelHandles);
            }
        }
        
        public void Dispose()
        {
            Jobs.Dispose();
            m_ParallelHandles.Dispose();
        }
    }
}

