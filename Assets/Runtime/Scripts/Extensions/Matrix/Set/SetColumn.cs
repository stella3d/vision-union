namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static void SetColumn<T>(this T[,] matrix, T[] column, int columnIndex)
        {
            var height = matrix.GetLength(0);
            for (var r = 0; r < height; r++)
            {
                matrix[r, columnIndex] = column[r];
            }
        }
    }
}