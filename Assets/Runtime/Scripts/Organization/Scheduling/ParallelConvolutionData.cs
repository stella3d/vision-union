using System;
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
    public class ParallelConvolutionData<TData> : IDisposable
        where TData: struct
    {
        // TODO - split out or merge the convolution + image data with the jobs, since the jobs duplicate all that
        public class Channel
        {
            public ImageData<TData> Input;
            public Sequence[] Sequences;
            
            public Sequence this [int i] => Sequences[i];
        }
        
        public class Sequence
        {
            public ImageData<TData> Output;
            public ConvolutionSequence<TData> Convolution;

            public Convolution<TData> this[int i] => Convolution[i];
        }

        /// <summary>
        /// The definition of the set of convolution sequences to run
        /// </summary>
        public readonly ParallelConvolutions<TData>[] Convolutions;

        /// <summary>
        /// The image outputs of each sequence
        /// </summary>
        public readonly ImageData<TData>[][] OutputImages;

        public Channel[] Channels;

        public readonly ImageData<TData>[] InputImages;

        public Channel this[int index] => Channels[index];
        
        public Sequence this[int channelIndex, int sequence]
        {
            get
            {
                var channel = Channels[channelIndex];
                return channel[sequence];
            }
        }
        
        public Convolution<TData> this[int channel, int sequence, int sequenceIndex]
        {
            get
            {
                var c = Channels[channel];
                var s = c.Sequences[sequence];
                return s[sequenceIndex];
            }
        }

        public int channelCount => Channels.Length;
        public int convolutionsPerChannel => Convolutions[0][0].Length;

        const int k_MaxSequences = 32;
        protected readonly NativeList<JobHandle> m_ParallelHandles = 
            new NativeList<JobHandle>(k_MaxSequences, Allocator.Persistent);
        
        public ParallelConvolutionData(ImageData<TData> input, 
            ParallelConvolutions<TData> convolution)
        {
            InputImages = new [] { input };
            Convolutions = new ParallelConvolutions<TData>[3];
            Convolutions[0] = convolution;

            var pad = Convolutions[0][0, 0].Padding;

            OutputImages = new ImageData<TData>[InputImages[0].Width][];
            InitializeImageData(input.Width - pad.x * 2, input.Height - pad.y * 2);
        }
        
        
        /*
        // TODO - probably factor this into a more specific class ?
        protected ParallelConvolutionData(ImageData<TData>[] images,
            ParallelConvolutions<TData>[] convolutions)
        {
            InputImages = images;
            Convolutions = convolutions;

            var pad = convolutions[0][0, 0].Padding;

            OutputImages = new ImageData<TData>[InputImages[0].Width][];
            var firstInput = InputImages[0];
            
            InitializeImageData(firstInput.Width - pad.x * 2, firstInput.Height - pad.y * 2);
            
            InitializeJobs();
        }
        
        protected ParallelConvolutionData(ImageData<TData> input, 
            ParallelConvolutions<TData>[] convolutions)
        {
            InputImages = new [] { input };
            Convolutions = convolutions;

            var pad = convolutions[0][0, 0].Padding;

            OutputImages = new ImageData<TData>[InputImages[0].Width][];
            InitializeImageData(input.Width - pad.x * 2, input.Height - pad.y * 2);
            
            InitializeJobs();
        }

        public JobHandle Schedule(JobHandle dependency)
        {
            m_ParallelHandles.Clear();
            var handle = dependency;
            foreach (var channel in Convolutions)
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
            for (var i = 0; i < Convolutions.Length; i++)
            {
                var channel = Convolutions[i];
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
            for (var i = 0; i < Convolutions.Length; i++)
            {
                var channel = Convolutions[i];
                var dependency = dependencies[i];
                m_ParallelHandles.Clear();
                foreach (JobSequence<TJob> jobSequence in channel)
                {
                    m_ParallelHandles.Add(jobSequence.Schedule(dependency));
                }
                
                handles[i] = JobHandle.CombineDependencies(m_ParallelHandles);
            }
        }
        
        */
        
        public void InitializeImageData(int width, int height)
        {
            foreach (var channel in OutputImages)
            {
                for (var i = 0; i < OutputImages.Length; i++)
                {
                    var existing = channel[i];
                    if (existing != default(ImageData<TData>) && existing.Buffer.IsCreated)
                        existing.Dispose();

                    channel[i] = new ImageData<TData>(width, height);
                }
            }
        }
        
        public void Dispose()
        {
            m_ParallelHandles.Dispose();
            OutputImages[0].Dispose();
        }
    }
}

