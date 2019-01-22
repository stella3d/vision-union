using System.Collections;
using Unity.Jobs;

namespace VisionUnion.Organization
{
    /// <summary>
    /// A set of jobs that run serially
    /// </summary>
    /// <typeparam name="T">The kernel type of the convolution</typeparam>
    public class JobSequence<T> : IEnumerable
        where T: struct, IJob
    {
        public T[] Jobs;
        
        public int Length => Jobs.Length;

        public JobSequence(T job)
        {
            Jobs = new[] { job };
        }
        
        public JobSequence(T[] jobs)
        {
            Jobs = jobs;
        }

        public IEnumerator GetEnumerator()
        {
            return Jobs.GetEnumerator();
        }
    }
}