namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
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
    }
}