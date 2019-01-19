using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    public struct ImageData<TPixelData>
        where TPixelData: struct
    {
        public int Width;
        public int Height;
        public NativeArray<TPixelData> Buffer;

        public ImageData(Texture2D texture)
        {
            Width = texture.width;
            Height = texture.height;
            Buffer = texture.GetRawTextureData<TPixelData>();
        }
    }
}