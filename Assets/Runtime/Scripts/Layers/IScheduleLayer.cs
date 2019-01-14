using Unity.Jobs;

namespace BurstVision
{
    public interface IScheduleLayer
    {
        JobHandle Handle { get; }

        JobHandle Schedule();
    }
}