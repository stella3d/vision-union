using System;

namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static double[,] ToDouble(this byte[,] matrix)
        {
            return matrix.Map((r, c) => Convert.ToDouble(matrix[r, c]));
        }
        
        public static double[,] ToDouble(this short[,] matrix)
        {
            return matrix.Map((r, c) => Convert.ToDouble(matrix[r, c]));
        }
        
        public static double[,] ToDouble(this float[,] matrix)
        {
            return matrix.Map((r, c) => Convert.ToDouble(matrix[r, c]));
        }
    }
}