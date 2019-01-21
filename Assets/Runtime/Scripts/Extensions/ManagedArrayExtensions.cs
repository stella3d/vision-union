using System;
using System.Linq;
using System.Text;
using Accord;
using Accord.Math;

namespace VisionUnion
{
    public static class ManagedArrayExtensions
    {
        public static short[] ToShort(this byte[] array)
        {
            return array.Select((b, i) => Convert.ToInt16(b)).ToArray();
        }
        
        public static float[] ToFloat(this byte[] array)
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