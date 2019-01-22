﻿using System.Collections;
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
        
        public JobSequence(int count)
        {
            Jobs = new T[count];
        }

        public JobSequence(T job)
        {
            Jobs = new[] { job };
        }
        
        public JobSequence(T[] jobs)
        {
            Jobs = jobs;
        }
        
        public T this[int index]
        {
            get { return Jobs[index]; }
            set { Jobs[index] = value;}
        }

        public IEnumerator GetEnumerator()
        {
            return Jobs.GetEnumerator();
        }
        
        public JobHandle Schedule(JobHandle dependency)
        {
            var handle = dependency;
            foreach (var job in Jobs)
            {
                handle = job.Schedule(handle);
            }

            return handle;
        }
    }
}