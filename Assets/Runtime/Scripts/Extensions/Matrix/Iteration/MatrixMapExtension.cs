using System;

namespace VisionUnion
{
    public static partial class ManagedMatrixExtensions
    {
        public static TOut[,] Map<TIn, TOut>(this TIn[,] matrix, Func<int, int, TOut> function)
        {
            var width = matrix.GetLength(0);
            var height = matrix.GetLength(1);
            var output = new TOut[height,width];
            for (var r = 0; r < height; r++)
            {
                for (var c = 0; c < width; c++)
                {
                    output[r, c] = function(r, c);
                }
            }

            return output;
        }
    }
}