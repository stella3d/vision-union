namespace BurstVision
{
    public static class ShortKernels
    {
        public static class Sobel
        {
            public static readonly short[,] X = 
            {
                {-1, 0, 1},
                {-2, 0, 2},
                {-1, 0, 1}
            };
            
            public static readonly short[] xVertical = { 1, 2, 1 };
            
            public static readonly short[] xHorizontal = { -1, 0, 1 };
            
            public static readonly short[,] Y = 
            {
                {-1, -2, -1},
                {0, 0, 0},
                {1, 2, 1}
            };
            
            public static readonly short[] yVertical = { -1, 0, 1 };

            public static readonly short[] yHorizontal = { 1, 2, 1 };
        }
    }
}