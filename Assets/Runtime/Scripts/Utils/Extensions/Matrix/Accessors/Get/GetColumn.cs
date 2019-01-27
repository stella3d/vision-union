namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static T[] GetColumn<T>(this T[,] matrix, int column)
        {
            var height = matrix.GetLength(0);
            var columnData = new T[height];
            for (var r = 0; r < height; r++)
            {
                columnData[r] = matrix[r, column];
            }

            return columnData;
        }
    }
}