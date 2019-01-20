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

                public static readonly short[] xVertical = {1, 2, 1};

                public static readonly short[] xHorizontal = {-1, 0, 1};

                public static readonly short[,] Y =
                {
                    {-1, -2, -1},
                    {0, 0, 0},
                    {1, 2, 1}
                };

                public static readonly short[] yVertical = {-1, 0, 1};

                public static readonly short[] yHorizontal = {1, 2, 1};
            }

            public static class Scharr
            {
                public static readonly short[,] X =
                {
                    {47, 0, -47},
                    {162, 0, -162},
                    {47, 0, -47}
                };

                public static readonly short[,] Y =
                {
                    {47, 162, 47},
                    {0, 0, 0},
                    {-47, -162, -47}
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