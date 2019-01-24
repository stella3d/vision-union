using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    public static partial class Texture2dExtensions
    {
        public static void LoadImageData(this Texture2D texture, ImageData<float> data, bool apply = true)
        {
            texture.LoadRawTextureData(data.Buffer);
            if(apply)
                texture.Apply();
        }

        public static void LoadImageData<T>(this Texture2D texture, NativeArray<T> buffer, bool apply = true)
            where T: struct
        {
            texture.LoadRawTextureData(buffer);
            if(apply)
                texture.Apply();
        }
    }
}
