using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using VisionUnion;
using Debug = UnityEngine.Debug;

public abstract class JobBenchmarkBehavior<TJob> : MonoBehaviour
    where TJob: struct, IJob, IDisposable
{
    public TJob job;
    public JobHandle jobHandle;

    public float immediateCompletionTimeTicks;
    public float compileAndScheduleTimeTicks;

    protected void Start()
    {
        job = CreateJob();
        
        // how long does compiling + scheduling take ?
        Util.Stopwatch.Restart();
        jobHandle = job.Schedule();
        Util.Stopwatch.Stop();
        compileAndScheduleTimeTicks = Util.Stopwatch.ElapsedTicks;
        
        // how long does actual execution take ?
        Util.Stopwatch.Restart();
        jobHandle.Complete();
        Util.Stopwatch.Stop();
        immediateCompletionTimeTicks = Util.Stopwatch.ElapsedTicks;
    }

    void Update()
    {
        enabled = false;
    }

    void OnDisable()
    {
        Debug.LogFormat("compile + schedule ticks: {0} , execution ticks: {1}", 
            compileAndScheduleTimeTicks, immediateCompletionTimeTicks);
        
        Util.Stopwatch.Reset();
        job.Dispose();
    }
    
    protected abstract TJob CreateJob();
}


internal static class Util
{
    public static readonly Stopwatch Stopwatch = new Stopwatch();
    
    public static Image<float> RandomFloatImage(int width, int height, float min = -1f, float max = 1f, 
        Allocator allocator = Allocator.TempJob)
    {
        var img = new Image<float>(width, height, allocator);
        for (var i = 0; i < img.Buffer.Length; i++)
        {
            img.Buffer[i] = UnityEngine.Random.Range(min, max);
        }

        return img;
    }
    
    public static Image<float3> RandomFloat3Image(int width, int height, float min = -1f, float max = 1f, 
        Allocator allocator = Allocator.TempJob)
    {
        var img = new Image<float3>(width, height, allocator);
        for (var i = 0; i < img.Buffer.Length; i++)
        {
            var r = UnityEngine.Random.Range(min, max);
            var g = UnityEngine.Random.Range(min, max);
            var b = UnityEngine.Random.Range(min, max);
            img.Buffer[i] = new float3(r, g, b);
        }

        return img;
    }
}