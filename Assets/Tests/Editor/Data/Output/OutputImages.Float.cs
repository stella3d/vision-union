namespace VisionUnion.Tests
{
    internal static partial class OutputImages
    {
        // TODO - fix these expected results for new padding results
        internal static readonly float[] Post3x3BoxBlur5x5 =
        {
            0.00f, 0.00f, 0.00f, 0.00f, 0.00f,
            0.00f, 7.33f, 6.00f, 4.00f, 0.00f,
            0.00f, 6.56f, 6.11f, 4.78f, 0.00f,
            0.00f, 5.56f, 6.00f, 5.33f, 0.00f,
            0.00f, 0.00f, 0.00f, 0.00f, 0.00f
        };
        
        internal static readonly float[] Post3x3GaussianBlur5x5 =
        {
            0.00f, 0.00f, 0.00f, 0.00f, 0.00f,
            0.00f, 7.50f, 6.19f, 3.81f, 0.00f,
            0.00f, 6.69f, 6.31f, 4.81f, 0.00f,
            0.00f, 5.69f, 6.00f, 5.50f, 0.00f,
            0.00f, 0.00f, 0.00f, 0.00f, 0.00f
        };
    }
}