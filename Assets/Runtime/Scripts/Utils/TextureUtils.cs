using UnityEngine;

namespace VisionUnion.Visualization.Utils
{
    public class TextureUtils
    {
        public static Texture2D SetupImage<T>(int width, int height, out Image<T> data, TextureFormat format)
            where T: struct
        {
            var texture = new Texture2D(width, height, format, false);
            data = new Image<T>(texture);
            return texture;
        }
    }
}