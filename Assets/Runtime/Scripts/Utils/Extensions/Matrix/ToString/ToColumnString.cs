namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
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
    }
}