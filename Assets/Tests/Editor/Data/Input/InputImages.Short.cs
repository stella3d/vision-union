using System;
using System.Linq;

namespace VisionUnion.Tests
{
    // short-type image examples are converted from byte examples to maintain consistency between types
    internal static partial class InputImages
    {
        internal static readonly short[] Short5x5 = Byte5x5.Select((b, i) => Convert.ToInt16(b)).ToArray();
        
        internal static readonly short[] Short6x6 = Byte6x6.Select((b, i) => Convert.ToInt16(b)).ToArray();
    }
}