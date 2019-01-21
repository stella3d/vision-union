namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static T[] GetRow<T>(this T[,] matrix, int row)
        {
            var width = matrix.GetLength(1);
            var rowData = new T[width];
            for (var c = 0; c < width; c++)
            {
                rowData[c] = matrix[row, c];
            }

            return rowData;
        }
    }
}