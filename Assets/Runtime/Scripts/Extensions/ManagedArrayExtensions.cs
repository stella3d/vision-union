using System;
using System.Linq;
using System.Text;
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
        
        public static float[] ToFloatNormalized(this byte[] array)
        {
            // 0-255 byte range --> 0-1 float range
            const float factor = 1/255f;
            return array.Select((b, i) => Convert.ToSingle(b) * factor).ToArray();
        }
        
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

        static readonly StringBuilder k_String = new StringBuilder(256);
        
        /*
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
        */
        
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
        
        public static double[] GetColumn(this double[,] matrix, int column)
        {
            var Height = matrix.GetLength(0);
            var columnData = new double[Height];
            for (var r = 0; r < Height; r++)
            {
                columnData[r] = matrix[r, column];
            }

            return columnData;
        }
        
        public static double[] GetRow(this double[,] matrix, int row)
        {
            var Width = matrix.GetLength(1);
            var rowData = new double[Width];
            for (var c = 0; c < Width; c++)
            {
                rowData[c] = matrix[row, c];
            }

            return rowData;
        }
    }
}