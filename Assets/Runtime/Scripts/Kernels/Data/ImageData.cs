using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    /// <summary>
    /// The data we need about an image / texture, in a form we can pass to a job 
    /// </summary>
    /// <typeparam name="TPixelData">The type of data that represents a single pixel</typeparam>
    public struct ImageData<TPixelData>
        where TPixelData: struct
    {
        public readonly int Width;
        public readonly int Height;
        public NativeArray<TPixelData> Buffer;

        public ImageData(Texture2D texture)
        {
            Width = texture.width;
            Height = texture.height;
            Buffer = texture.GetRawTextureData<TPixelData>();
        }
    }
}