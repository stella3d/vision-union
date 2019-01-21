using System;
using System.Linq;

namespace VisionUnion
{
    public static partial class ManagedArrayExtensions
    {
        public static short[] ToShort(this byte[] array)
        {
            return array.Select((b, i) => Convert.ToInt16(b)).ToArray();
        }
        
        public static short[] ToShort(this int[] array)
        {
            return array.Select((b, i) => Convert.ToInt16(b)).ToArray();
        }
        
        public static short[] ToShort(this float[] array)
        {
            return array.Select((b, i) => Convert.ToInt16(b)).ToArray();
        }
        
        public static short[] ToShort(this double[] array)
        {
            return array.Select((b, i) => Convert.ToInt16(b)).ToArray();
        }
    }
}