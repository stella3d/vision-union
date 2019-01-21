using System;

namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static float[,] ToFloat(this double[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);
            var outMatrix = new float[rows, columns];
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    outMatrix[r, c] = Convert.ToSingle(matrix[r, c]);
                }
            }

            return outMatrix;
        }
    }
}