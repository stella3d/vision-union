using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    public static partial class Texture2dExtensions
    {
        public static void LoadImageData(this Texture2D texture, Image<byte> data, bool apply = true)
        {
            texture.LoadRawTextureData(data.Buffer);
            if(apply)
                texture.Apply();
        }
        
        public static void LoadImageData(this Texture2D texture, Image<float> data, bool apply = true)
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
        
        
        
        public static Texture2D SetupImage<T>(int width, int height, out Image<T> data, TextureFormat format)
            where T: struct
        {
            var texture = new Texture2D(width, height, format, false);
            data = new Image<T>(texture);
            return texture;
        }
    }
}
