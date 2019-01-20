using System;
using System.Linq;

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
        
        public static float[] ToFloatNormalized(this byte[] array)
        {
            // 0-255 byte range --> 0-1 float range
            const float factor = 1/255f;
            return array.Select((b, i) => Convert.ToSingle(b) * factor).ToArray();
        }
        
        public static double[,] ToDouble(this short[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);
            var outMatrix = new double[rows, columns];
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    outMatrix[r, c] = Convert.ToDouble(matrix[r, c]);
                }
            }

            return outMatrix;
        }
        
        public static double[,] ToDouble(this float[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);
            var outMatrix = new double[rows, columns];
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    outMatrix[r, c] = Convert.ToDouble(matrix[r, c]);
                }
            }

            return outMatrix;
        }
    }
}