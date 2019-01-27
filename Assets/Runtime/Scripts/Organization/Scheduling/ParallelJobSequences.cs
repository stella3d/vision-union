using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Organization
{
    public class ParallelJobSequences<T> : IEnumerable<JobSequence<T>> , IDisposable
        where T: struct, IJob
    {
        public readonly JobSequence<T>[] Sequences;

        public int Width => Sequences.Length;
        
        public ParallelJobSequences(int capacity, int sequenceLength)
        {
            Sequences = new JobSequence<T>[capacity];
            for (var i = 0; i < Sequences.Length; i++)
            {
                Sequences[i] = new JobSequence<T>(sequenceLength);
            }
        }
        
        public ParallelJobSequences(int capacity, int[] sequenceLengths)
        {
            Sequences = new JobSequence<T>[capacity];
            for (var i = 0; i < Sequences.Length; i++)
            {
                Sequences[i] = new JobSequence<T>(sequenceLengths[i]);
            }
        }
        
        public ParallelJobSequences(JobSequence<T> sequence)
        {
            Sequences = new [] { sequence };
        }
        
        public ParallelJobSequences(JobSequence<T>[] sequences)
        {
            Sequences = sequences;
        }
        
        public JobSequence<T> this[int sequence]
        {
            get { return Sequences[sequence]; }
            set { Sequences[sequence] = value; }
        }

        public T this[int sequence, int sequenceIndex]
        {
            get { return Sequences[sequence][sequenceIndex]; }
            set { Sequences[sequence][sequenceIndex] = value; }
        }
        
        const int k_MaxSequences = 32;
        protected readonly NativeList<JobHandle> m_ParallelHandles = 
            new NativeList<JobHandle>(k_MaxSequences, Allocator.Persistent);
        
        public JobHandle Schedule(JobHandle dependency)
        {
            m_ParallelHandles.Clear();
            var handle = dependency;
            foreach (var jobSequence in Sequences)
            {
                m_ParallelHandles.Add(jobSequence.Schedule(handle));
            }

            handle = JobHandle.CombineDependencies(m_ParallelHandles);
            return handle;
        }

        IEnumerator<JobSequence<T>> IEnumerable<JobSequence<T>>.GetEnumerator()
        {
            return (IEnumerator<JobSequence<T>>)Sequences.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Sequences.GetEnumerator();
        }

        public void Dispose()
        {
            m_ParallelHandles.Dispose();
        }
    }
}

