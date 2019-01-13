namespace BurstVision
{
    public static class ShortKernels
    {
        public static class Sobel
        {
            public static readonly short[,] Vertical = 
            {
                {-1, 0, 1},
                {-2, 0, 2},
                {-1, 0, 1}
            };
            
            public static readonly short[,] Horizontal = 
            {
                {-1, -2, -1},
                {0, 0, 0},
                {1, 2, 1}
            };
        }
    }
}