namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static void SetRow<T>(this T[,] matrix, T[] row, int rowIndex)
        {
            var width = matrix.GetLength(1);
            for (var c = 0; c < width; c++)
            {
                matrix[rowIndex, c] = row[c];
            }
        }
    }
}