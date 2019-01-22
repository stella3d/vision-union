using System.Collections;
using Unity.Jobs;

namespace VisionUnion.Organization
{
    public class ParallelJobSequences<T> : IEnumerable 
        where T: struct, IJob
    {
        public readonly JobSequence<T>[] Sequences;

        public int Width => Sequences.Length;
        
        public ParallelJobSequences(JobSequence<T> sequence)
        {
            Sequences = new [] { sequence };
        }
        
        public ParallelJobSequences(JobSequence<T>[] sequences)
        {
            Sequences = sequences;
        }

        public T this[int sequence, int sequenceIndex]
        {
            get { return Sequences[sequence][sequenceIndex]; }
            set { Sequences[sequence][sequenceIndex] = value; }
        }
        
        public IEnumerator GetEnumerator()
        {
            return Sequences.GetEnumerator();
        }
    }
}

