using Unity.Collections;
using Unity.Mathematics;

namespace VisionUnion
{
    public static class ImageNormalizationExtensions
    {
        public static float2 FindMinMax(this NativeArray<float> data)
        {
            var minimum = float.MaxValue;
            var maximum = float.MinValue;
            foreach (var inputValue in data)
            {
                minimum = math.select(inputValue, minimum, inputValue < minimum);
                maximum = math.select(inputValue, maximum, inputValue > maximum);
            }
    
            return new float2(minimum, maximum);
        }
    }
}
