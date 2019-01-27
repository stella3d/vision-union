namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
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
    }
}