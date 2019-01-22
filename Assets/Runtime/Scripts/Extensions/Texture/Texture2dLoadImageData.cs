using UnityEngine;

namespace VisionUnion
{
    public static partial class Texture2dExtensions
    {
        public static void LoadImageData(this Texture2D texture, ImageData<float> data)
        {
            texture.LoadRawTextureData(data.Buffer);
            texture.Apply();
        }
    }
}
