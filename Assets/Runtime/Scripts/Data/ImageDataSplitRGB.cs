using System;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion
{
    
    public struct ImageDataSplitRGB<T> : IDisposable
        where T: struct
    {
        public Image<T> r;
        public Image<T> g;
        public Image<T> b;
        
        static readonly Image<T>[] k_Channels  = new Image<T>[3];
        
        public ImageDataSplitRGB(Image<T> r, Image<T> g, Image<T> b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        
        public ImageDataSplitRGB(NativeArray<T> r, NativeArray<T> g, NativeArray<T> b, 
            int width, int height)
        {
            this.r = new Image<T>(r, width, height);
            this.g = new Image<T>(g, width, height);
            this.b = new Image<T>(b, width, height);
        }
        
        public Image<T>? this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return r;
                    case 1:
                        return g;
                    case 2:
                        return b;
                }

                return null;
            }
        }
        
        public JobHandle ScheduleChannels(Func<Image<T>, int, JobHandle, JobHandle> scheduleFunction, JobHandle dependency)
        {
            var redHandle = scheduleFunction(r, 1, dependency);
            var greenHandle = scheduleFunction(g, 2, dependency);
            var blueHandle = scheduleFunction(g, 3, dependency);
            return JobHandle.CombineDependencies(redHandle, greenHandle, blueHandle);
        }

        public void Dispose()
        {
            r.Dispose();
            g.Dispose();
            b.Dispose();
        }
    }
}