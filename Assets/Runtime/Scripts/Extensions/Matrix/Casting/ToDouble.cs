using System;

namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static double[,] ToDouble(this byte[,] matrix)
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