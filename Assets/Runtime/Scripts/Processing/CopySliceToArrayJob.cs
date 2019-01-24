using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion.Jobs
{
    public struct CopySliceToArrayJob<T>: IJob
        where T: struct
    {
        [ReadOnly] public NativeSlice<T> Input;
        [WriteOnly] public NativeArray<T> Output;
        
        public CopySliceToArrayJob(NativeSlice<T> input, NativeArray<T> output)
        {
            Input = input;
            Output = output;
        }

        public void Execute()
        { 
            Input.CopyTo(Output);
        }
    }
}