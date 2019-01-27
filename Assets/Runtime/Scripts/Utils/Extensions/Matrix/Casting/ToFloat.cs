using System;

namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static float[,] ToFloat(this byte[,] matrix)
        {
            return matrix.Map((r, c) => Convert.ToSingle(matrix[r, c]));
        }
        
        public static float[,] ToFloat(this double[,] matrix)
        {
            return matrix.Map((r, c) => Convert.ToSingle(matrix[r, c]));
        }
        
        public static float[,] ToFloat(this short[,] matrix)
        {
            return matrix.Map((r, c) => Convert.ToSingle(matrix[r, c]));
        }
    }
}