using System;

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
    }
}

