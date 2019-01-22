using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;

namespace VisionUnion.Organization
{
    public class ParallelJobSequences<T> : IEnumerable<T> 
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

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (IEnumerator<T>)Sequences.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Sequences.GetEnumerator();
        }
    }
}

