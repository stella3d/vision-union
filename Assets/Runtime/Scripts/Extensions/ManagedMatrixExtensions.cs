using System;
using System.Text;

namespace VisionUnion
{
    public static class ManagedMatrixExtensions
    {
        public static double[,] MultiplyWithRow(this double[] column, double[] row)
        {
            var rows = column.Length;
            var columns = row.Length;
            var outMatrix = new double[rows, columns];
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    var inC = column[r];
                    var inR = row[c];
                    outMatrix[r, c] = inC * inR;
                }
            }

            return outMatrix;
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
        
        public static string ToColumnString(this double[] column)
        {
            k_String.Clear();
            foreach (var t in column)
            {
                k_String.AppendLine(t.ToString("F3"));
            }
            
            return k_String.ToString();
        }
        
        public static string ToColumnString(this float[] column)
        {
            k_String.Clear();
            foreach (var t in column)
            {
                k_String.AppendLine(t.ToString("F3"));
            }
            
            return k_String.ToString();
        }
        
        public static string ToColumnString(this short[] column)
        {
            k_String.Clear();
            foreach (var t in column)
            {
                k_String.AppendLine(t.ToString("F3"));
            }
            
            return k_String.ToString();
        }
        
        public static string ToRowString(this double[] row)
        {
            k_String.Clear();
            var last = row.Length - 1;
            for (var i = 0; i < last; i++)
            {
                var t = row[i];
                k_String.AppendFormat("{0:F3}, ", t);
            }

            k_String.AppendFormat("{0:F3}\n", row[last]);
            return k_String.ToString();
        }
        
        public static string ToRowString(this float[] row)
        {
            k_String.Clear();
            var last = row.Length - 1;
            for (var i = 0; i < last; i++)
            {
                var t = row[i];
                k_String.AppendFormat("{0:F3}, ", t);
            }

            k_String.AppendFormat("{0:F3}\n", row[last]);
            return k_String.ToString();
        }
        
        public static double[] GetColumn(this double[,] matrix, int column)
        {
            var height = matrix.GetLength(0);
            var columnData = new double[height];
            for (var r = 0; r < height; r++)
            {
                columnData[r] = matrix[r, column];
            }

            return columnData;
        }
        
        public static void SetRow<T>(this T[,] matrix, T[] row, int rowIndex)
        {
            var width = matrix.GetLength(1);
            for (var c = 0; c < width; c++)
            {
                matrix[rowIndex, c] = row[c];
            }
        }
        
        public static void SetColumn<T>(this T[,] matrix, T[] column, int columnIndex)
        {
            var height = matrix.GetLength(0);
            for (var r = 0; r < height; r++)
            {
                matrix[r, columnIndex] = column[r];
            }
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