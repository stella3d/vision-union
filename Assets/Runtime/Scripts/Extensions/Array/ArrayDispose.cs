using System;
using Unity.Collections;

namespace VisionUnion
{
    public static partial class ManagedArrayExtensions
    {
        public static void Dispose<T>(this T[] array) where T: IDisposable
        {
            foreach (var item in array)
            {
                item.Dispose();
            }
        }
        
        public static void DisposeIfCreated<T>(this NativeArray<T> array)
            where T: struct
        {
            if(array.IsCreated)
                array.Dispose();
        }
    }
}

