using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    public static class Texture2dExtensions
    {
        public static void LoadImageData(this Texture2D texture, ImageData<byte> data, bool apply = true)
        {
            texture.LoadRawTextureData(data.Buffer);
            if(apply)
                texture.Apply();
        }
        
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
        
        public static Texture2D SetupImage<T>(int width, int height, out ImageData<T> data, TextureFormat format)
            where T: struct
        {
            var texture = new Texture2D(width, height, format, false);
            data = new ImageData<T>(texture);
            return texture;
        }
    }
}
