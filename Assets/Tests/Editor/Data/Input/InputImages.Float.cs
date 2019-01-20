namespace VisionUnion.Tests
{
    // float-type image examples are converted from byte examples to maintain consistency between types
    internal static partial class InputImages
    {
        internal static readonly float[] Float5x5 = Byte5x5.ToFloat();
        internal static readonly float[] Float6x6 = Byte6x6.ToFloat();
    }
}