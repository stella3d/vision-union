namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
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
    }
}