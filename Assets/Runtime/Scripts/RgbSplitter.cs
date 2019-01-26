using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using VisionUnion;
using VisionUnion.Jobs;

public static class RgbSplitter
{
    public static NativeArray<TImage> AllocateRgbChannels<TImage, TChannel>(Texture2D input, Allocator allocator,
        out ImageDataSplitRGB<TChannel> rgb)
        where TImage: struct
        where TChannel: struct
    {
        var data = input.GetRawTextureData<TImage>();
        var red = new NativeArray<TChannel>(data.Length, allocator);    
        var green = new NativeArray<TChannel>(data.Length, allocator);    
        var blue = new NativeArray<TChannel>(data.Length, allocator);
        rgb = new ImageDataSplitRGB<TChannel>(red, green, blue, input.width, input.height);
        return data;
    }

    public static void PrepareOutputTextures(Texture2D input, 
        out Texture2D red, out Texture2D green, out Texture2D blue,
        TextureFormat format = TextureFormat.Alpha8)
    {
        red = new Texture2D(input.width, input.height, format, false);
        green = new Texture2D(input.width, input.height, format, false);
        blue = new Texture2D(input.width, input.height, format, false);
    }

    public static JobHandle ScheduleArraySplit(NativeArray<Color24> image, NativeArray<byte>[] split)
    {
        var rgb = image.Slice(0);
        var handles = new NativeList<JobHandle>(3, Allocator.Temp);
        for (var i = 0; i < 3; i++)
        {
            var job = new CopySliceToArrayJob<byte>(rgb.SliceWithStride<byte>(i), split[i]);
            handles.Add(job.Schedule());
        }

        var handle = JobHandle.CombineDependencies(handles);
        handles.Dispose();
        return handle;
    }
    
    public static JobHandle ScheduleArraySplit(NativeArray<Color24> image, ImageData<byte>[] split)
    {
        var rgb = image.Slice(0);
        var handles = new NativeList<JobHandle>(3, Allocator.Temp);
        for (var i = 0; i < 3; i++)
        {
            var job = new CopySliceToArrayJob<byte>(rgb.SliceWithStride<byte>(i), split[i].Buffer);
            handles.Add(job.Schedule());
        }

        var handle = JobHandle.CombineDependencies(handles);
        handles.Dispose();
        return handle;
    }
}
