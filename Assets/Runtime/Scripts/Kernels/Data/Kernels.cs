namespace BurstVision
{
    public static class Kernels
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
        
        public static readonly float[,] BoxBlur = 
        {
            {1/9f, 1/9f, 1/9f},
            {1/9f, 1/9f, 1/9f},
            {1/9f, 1/9f, 1/9f}
        };
        
        public static readonly float[,] GaussianBlurApproximate3x3 = 
        {
            {1/16f, 2/16f, 1/16f},
            {2/16f, 4/16f, 2/16f},
            {1/16f, 2/16f, 1/16f}
        };
        
        public static readonly float[,] GaussianBlurApproximate5x5 = 
        {
            {1/256f, 4/256f, 6/256f, 4/256f, 1/256f},
            {4/256f, 16/256f, 24/256f, 16/256f, 4/256f},
            {6/256f, 24/256f, 36/256f, 24/256f, 6/256f},
            {4/256f, 16/256f, 24/256f, 16/256f, 4/256f},
            {1/256f, 4/256f, 6/256f, 4/256f, 1/256f}
        };
    }
}