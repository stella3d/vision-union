namespace VisionUnion
{
    public static partial class Kernels
    {
        public static class Short
        {
            public static readonly short[,] Identity1x1 =
            {
                { 1 }
            };    

            public static readonly short[,] Identity3x3 =
            {
                {0, 0, 0},
                {0, 1, 0},
                {0, 0, 0}
            };

            public static class Sobel
            {
                public static readonly short[,] X =
                {
                    {-1, 0, 1},
                    {-2, 0, 2},
                    {-1, 0, 1}
                };
                
                public static readonly short[,] x5x5 =
                {
                    {-5, -4, 0, 4, 5},
                    {-8, -10, 0, 10,8},
                    {-10, -20, 0, 20, 10},
                    {-8, -10, 0, 10, 8},
                    {-5, -4, 0, 4, 5}
                };

                public static readonly short[] xVertical = {1, 2, 1};

                public static readonly short[] xHorizontal = {-1, 0, 1};

                public static readonly short[,] Y =
                {
                    {-1, -2, -1},
                    {0, 0, 0},
                    {1, 2, 1}
                };
                
                public static readonly short[,] y5x5 =
                {
                    {5, 8, 10, 8, 5},
                    {4, 10, 20, 10, 4},
                    {0, 0, 0, 0, 0},
                    {-4, -10, -20, 10, -4},
                    {-5, -8, -10, -8, -5}
                };

                public static readonly short[] yVertical = {-1, 0, 1};

                public static readonly short[] yHorizontal = {1, 2, 1};
            }

            public static class Scharr
            {
                public static readonly short[,] X =
                {
                    {3, 0, -3},
                    {10, 0, -10},
                    {3, 0, -3}
                };

                public static readonly short[,] Y =
                {
                    {3, 10, 3},
                    {0, 0, 0},
                    {-3, -10, -3}
                };
            }

            public static readonly short[,] Sharpen =
            {
                {0, -1, 0},
                {-1, 5, -1},
                {0, -1, 0}
            };

            public static readonly short[,] Emboss =
            {
                {-2, -1, 0},
                {-1, 1, 1},
                {0, 1, 2}
            };

            public static readonly short[,] Outline =
            {
                {-1, -1, -1},
                {-1, 8, -1},
                {-1, -1, -1}
            };
        }
    }
}