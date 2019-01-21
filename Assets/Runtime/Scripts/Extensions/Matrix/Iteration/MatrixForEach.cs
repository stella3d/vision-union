using System;

namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static void ForEach<T>(this T[,] matrix, Action<int, int> action)
        {
            var columnCount = matrix.GetLength(0);
            for (var r = 0; r < matrix.GetLength(1); r++)
            {
                for (var c = 0; c < columnCount; c++)
                {
                    action(r, c);
                }
            }
        }
    }
}