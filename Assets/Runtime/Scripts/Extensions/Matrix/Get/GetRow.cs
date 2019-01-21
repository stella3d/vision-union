namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
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