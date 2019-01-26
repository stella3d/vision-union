using System;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion
{
    
    public struct ImageDataSplitRGB<T> : IDisposable
        where T: struct
    {
        public ImageData<T> r;
        public ImageData<T> g;
        public ImageData<T> b;
        
        static readonly ImageData<T>[] k_Channels  = new ImageData<T>[3];
        
        public ImageDataSplitRGB(ImageData<T> r, ImageData<T> g, ImageData<T> b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        
        public ImageDataSplitRGB(NativeArray<T> r, NativeArray<T> g, NativeArray<T> b, 
            int width, int height)
        {
            this.r = new ImageData<T>(r, width, height);
            this.g = new ImageData<T>(g, width, height);
            this.b = new ImageData<T>(b, width, height);
        }
        
        public ImageData<T>? this[int index]
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
        
        public JobHandle ScheduleChannels(Func<ImageData<T>, int, JobHandle, JobHandle> scheduleFunction, JobHandle dependency)
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