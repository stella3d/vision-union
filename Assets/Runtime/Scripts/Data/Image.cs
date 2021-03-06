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
    public struct Image<TPixelData> : IDisposable, 
        IEquatable<Image<TPixelData>>
        where TPixelData: struct
    {
        public readonly int Width;
        public readonly int Height;
        public NativeArray<TPixelData> Buffer;

        /// <summary>
        /// Create a representation of this texture
        /// </summary>
        /// <param name="texture">The source texture</param>
        /// <param name="copy">
        /// When false, will use the source texture's data.
        /// When true, will create a copy of the texture data, which is slower.</param>
        /// <param name="allocator">The allocator to use if copying.  Has no effect if not copying</param>
        public Image(Texture2D texture, bool copy = false, Allocator allocator = Allocator.Persistent)
        {
            if (!texture.isReadable)
            {
                Debug.LogWarningFormat("Texture {0} needs to be readable to be used as input!");
                Width = texture.width;
                Height = texture.height;
            }
            
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

        public Image(NativeArray<TPixelData> data, int width, int height)
        {
            Width = width;
            Height = height;
            Buffer = data;
        }
        
        public Image(int width, int height, Allocator allocator = Allocator.Persistent)
        {
            Width = width;
            Height = height;
            Buffer = new NativeArray<TPixelData>(Width * height, allocator);
        }
        
        public Image(TPixelData[] data, int width, int height, Allocator allocator = Allocator.Persistent)
        {
            Width = width;
            Height = height;
            Buffer = new NativeArray<TPixelData>(data, allocator);
        }
        
        public Texture2D ToTexture(TextureFormat format)
        {
            var texture = new Texture2D(Width, Height, format, false);
            texture.LoadRawTextureData(Buffer);
            return texture;
        }

        public void Dispose()
        {
            Buffer.DisposeIfCreated();
        }
        
        public static bool operator ==(Image<TPixelData> a, Image<TPixelData> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Image<TPixelData> a, Image<TPixelData> b)
        {
            return !a.Equals(b);
        }
        
        public bool Equals(Image<TPixelData> other)
        {
            return Width == other.Width && Height == other.Height && Buffer.Equals(other.Buffer);
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var cast = (Image<TPixelData>)other;
            return Equals(cast);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Width;
                hashCode = (hashCode * 397) ^ Height;
                hashCode = (hashCode * 397) ^ Buffer.GetHashCode();
                return hashCode;
            }
        }
    }
}