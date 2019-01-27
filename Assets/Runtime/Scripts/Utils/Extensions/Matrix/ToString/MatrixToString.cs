using System;
using System.Text;

namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        static readonly StringBuilder k_String = new StringBuilder(256);
        
        public static string MatrixToString<T>(this T[,] matrix)
        {
            k_String.Clear();
            var columns = matrix.GetLength(1);
            var rows = matrix.GetLength(0);
            var rowEnd = rows - 1;
            for (var r = 0; r < rowEnd; r++)
            {
                for (var c = 0; c < columns - 1; c++)
                {
                    k_String.AppendFormat("{0}, ", matrix[r, c]);
                }
                
                k_String.AppendFormat("{0},\n", matrix[r, columns - 1]);
            }
            
            for (var c = 0; c < columns - 1; c++)
            {
                k_String.AppendFormat("{0}, ", matrix[rowEnd, c]);
            }
                
            k_String.Append(matrix[rowEnd, columns - 1]);
            return k_String.ToString();
        }
        
        public static string MatrixToString(this float[,] matrix)
        {
            return MatrixToString(matrix.ToDouble());
        }
        
        public static string MatrixToString(this double[,] matrix)
        {
            k_String.Clear();
            var columns = matrix.GetLength(1);
            var rows = matrix.GetLength(0);
            var rowEnd = rows - 1;
            for (var r = 0; r < rowEnd; r++)
            {
                for (var c = 0; c < columns - 1; c++)
                {
                    k_String.AppendFormat("{0:F3}, ", matrix[r, c]);
                }
                
                k_String.AppendFormat("{0:F3},\n", matrix[r, columns - 1]);
            }
            
            for (var c = 0; c < columns - 1; c++)
            {
                k_String.AppendFormat("{0:F3}, ", matrix[rowEnd, c]);
            }
                
            k_String.Append(matrix[rowEnd, columns - 1].ToString("F3"));
            return k_String.ToString();
        }
    }
}