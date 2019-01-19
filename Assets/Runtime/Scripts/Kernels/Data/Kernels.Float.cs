namespace VisionUnion
{
    public static partial class Kernels
    {
        public static class Float
        {
            public static readonly float[,] BoxBlur =
            {
                {1f / 9f, 1f / 9f, 1f / 9f},
                {1f / 9f, 1f / 9f, 1f / 9f},
                {1f / 9f, 1f / 9f, 1f / 9f}
            };

            public static readonly float[,] GaussianBlurApproximate3x3 =
            {
                {1f / 16f, 2f / 16f, 1f / 16f},
                {2f / 16f, 4f / 16f, 2f / 16f},
                {1f / 16f, 2f / 16f, 1f / 16f}
            };

            public static readonly float[,] GaussianBlurApproximate5x5 =
            {
                {1 / 256f, 4 / 256f, 6 / 256f, 4 / 256f, 1 / 256f},
                {4 / 256f, 16 / 256f, 24 / 256f, 16 / 256f, 4 / 256f},
                {6 / 256f, 24 / 256f, 36 / 256f, 24 / 256f, 6 / 256f},
                {4 / 256f, 16 / 256f, 24 / 256f, 16 / 256f, 4 / 256f},
                {1 / 256f, 4 / 256f, 6 / 256f, 4 / 256f, 1 / 256f}
            };
        }
    }
}