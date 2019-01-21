using System;
using System.Linq;

namespace VisionUnion
{
    public static partial class ManagedArrayExtensions
    {
        public static float[] ToFloat(this byte[] array)
        {
            return array.Select((b, i) => Convert.ToSingle(b)).ToArray();
        }
        
        public static float[] ToFloat(this short[] array)
        {
            return array.Select((b, i) => Convert.ToSingle(b)).ToArray();
        }
        
        public static float[] ToFloat(this int[] array)
        {
            return array.Select((b, i) => Convert.ToSingle(b)).ToArray();
        }
        
        public static float[] ToFloat(this double[] array)
        {
            return array.Select((b, i) => Convert.ToSingle(b)).ToArray();
        }
        
        public static float[] ToFloatNormalized(this byte[] array)
        {
            // 0-255 byte range --> 0-1 float range
            const float factor = 1/255f;
            return array.Select((b, i) => Convert.ToSingle(b) * factor).ToArray();
        }
    }
}