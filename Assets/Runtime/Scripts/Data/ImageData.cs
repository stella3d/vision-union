using System;
using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    /// <summary>
    /// The data we need about an image / texture, in a form we can pass to a job 
    /// </summary>
    /// <typeparam name="TPixelData">
    /// The type of data that represents a single pixel. If there are multiple channels, the struct
    /// should have members representing each channel, whether sequential or explicit layout.
    /// </typeparam>
    public struct ImageData<TPixelData> : IDisposable
        where TPixelData: struct
    {
        public readonly int Width;
        public readonly int Height;
        public readonly NativeArray<TPixelData> Buffer;

        /// <summary>
        /// Create a representation of this texture
        /// </summary>
        /// <param name="texture">The source texture</param>
        /// <param name="copy">
        /// When false, will use the source texture's data.
        /// When true, will create a copy of the texture data, which is slower.</param>
        /// <param name="allocator">The allocator to use if copying.  Has no effect if not copying</param>
        public ImageData(Texture2D texture, bool copy = false, Allocator allocator = Allocator.Persistent)
        {
            Width = texture.width;
            Height = texture.height;
            if (copy)
            {
                Buffer = new NativeArray<TPixelData>(Width * Height, allocator);
                Buffer.CopyFrom(texture.GetRawTextureData<TPixelData>());
            }
            else
            {
                Buffer = texture.GetRawTextureData<TPixelData>();
            }
        }

        public void Dispose()
        {
            Buffer.Dispose();
        }
    }
}