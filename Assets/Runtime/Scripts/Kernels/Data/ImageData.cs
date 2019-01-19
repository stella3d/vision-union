using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    public struct ImageData<T>
        where T: struct
    {
        public int Width;
        public int Height;
        public NativeArray<T> Buffer;

        public ImageData(Texture2D texture)
        {
            Width = texture.width;
            Height = texture.height;
            Buffer = texture.GetRawTextureData<T>();
        }
    }
}