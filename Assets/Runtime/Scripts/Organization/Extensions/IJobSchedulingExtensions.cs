using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Organization
{
    public static class IJobSchedulingExtensions
    {
        public static JobHandle ScheduleArray<T>(this T[] jobs, JobHandle dependency)
            where T: struct, IJob
        {
            var handle = dependency;
            foreach (var job in jobs)
            {
                handle = job.Schedule(handle);
            }

            return handle;
        }
        
        public static readonly NativeList<JobHandle> k_ParallelHandles = new NativeList<JobHandle>(16, Allocator.Persistent);
        
        public static JobHandle ScheduleParallel<T>(this T[][] jobStructMatrix, JobHandle dependency)
            where T: struct, IJob
        {
            k_ParallelHandles.Clear();
            var handle = dependency;
            foreach (var jobSequence in jobStructMatrix)
            {
                k_ParallelHandles.Add(jobSequence.ScheduleArray(handle));
            }

            handle = JobHandle.CombineDependencies(k_ParallelHandles);
            return handle;
        }
    }
}

