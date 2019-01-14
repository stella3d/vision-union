using System.Collections.Generic;
using Unity.Jobs;

namespace BurstVision
{
    public class Conv2dNetwork<TInputData>
        where TInputData: struct
    {
        public readonly List<IScheduleLayer> LayerSchedule;

        public JobHandle Handle { get; protected set; }
        
        public Conv2dNetwork(List<IScheduleLayer> schedule)
        {
            LayerSchedule = schedule;
        }

        public void Schedule()
        {
            var previous = Handle;
            foreach (var layerScheduler in LayerSchedule)
            {
                // TODO - filters don't have to depend on each other
                previous = layerScheduler.Schedule();
            }

            Handle = previous;
        }
    }
}