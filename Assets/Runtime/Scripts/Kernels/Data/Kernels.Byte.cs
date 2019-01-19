namespace VisionUnion
{
    public static partial class Kernels
    {
        public static class Byte
        {
            public static readonly byte[,] Identity1x1 =
            {
                { 1 }
            };    

            public static readonly byte[,] Identity3x3 =
            {
                {0, 0, 0},
                {0, 1, 0},
                {0, 0, 0}
            };
        }
    }
}