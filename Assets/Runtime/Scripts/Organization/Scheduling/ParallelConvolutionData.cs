using System;
using System.Collections.Generic;
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

            public Channel(ImageData<TData> input, ImageData<TData>[] outputs, 
                ParallelConvolutions<TData> convolutions)
            {
                Input = input;
                Sequences = new Sequence[outputs.Length];
                for (var i = 0; i < outputs.Length; i++)
                {
                    Sequences[i] = new Sequence(outputs[i], convolutions[i]);
                }
            }
        }
        
        public class Sequence
        {
            public ImageData<TData> Output;
            public ConvolutionSequence<TData> Convolution;

            public Convolution2D<TData> this[int i] => Convolution[i];

            public Sequence(ImageData<TData> output, ConvolutionSequence<TData> convolution)
            {
                Output = output;
                Convolution = convolution;
            }
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
        
        public Sequence this[int channel, int sequence]
        {
            get
            {
                var c = Channels[channel];
                var seq = c.Sequences[sequence];
                if (seq != null) 
                    return seq;
                
                seq = new Sequence(OutputImages[channel][sequence], Convolutions[channel][sequence]);
                c.Sequences[sequence] = seq;
                return seq;
            }
        }
        
        public Convolution2D<TData> this[int channel, int sequence, int sequenceIndex]
        {
            get
            {
                var c = Channels[channel];
                var s = c.Sequences[sequence];
                return s[sequenceIndex];
            }
        }

        public int channelCount => Convolutions.Length;
        public int convolutionsPerChannel => Convolutions[0].Depth;

        const int k_MaxSequences = 32;
        protected readonly NativeList<JobHandle> m_ParallelHandles = 
            new NativeList<JobHandle>(k_MaxSequences, Allocator.Persistent);
        
        public ParallelConvolutionData(ImageData<TData> input, 
            ParallelConvolutions<TData> convolution)
        {
            InputImages = new [] { input };
            Convolutions = new[] { convolution };

            OutputImages = new ImageData<TData>[channelCount][];
            InitializeImageData(input.Width, input.Height);

            Channels = new[] { new Channel(input, OutputImages[0], convolution) };
        }
        
        public ParallelConvolutionData(ImageData<TData>[] inputs, 
            ParallelConvolutions<TData>[] convolutions)
        {
            InputImages = inputs;
            Convolutions = convolutions;
            OutputImages = new ImageData<TData>[channelCount][];
            
            var firstInput = inputs[0];
            InitializeImageData(firstInput.Width, firstInput.Height);

            Channels = new Channel[inputs.Length];
            for (var i = 0; i < Channels.Length; i++)
            {
                Channels[i] = new Channel(inputs[i], OutputImages[i], convolutions[i]);
            }
        }
        
        public void InitializeImageData(int width, int height)
        {
            for (var i = 0; i < OutputImages.Length; i++)
            {
                var arr = new ImageData<TData>[convolutionsPerChannel];
                OutputImages[i] = arr;
                for (var j = 0; j < arr.Length; j++)
                {
                    arr[j] = new ImageData<TData>(width, height);
                }
            }
        }
        
        public void Dispose()
        {
            m_ParallelHandles.Dispose();
            Convolutions.Dispose();
            foreach (var outImage in OutputImages)
            {
                outImage.Dispose();
            }
        }
    }
}

