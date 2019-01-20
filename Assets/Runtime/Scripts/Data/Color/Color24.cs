using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VisionUnion
{
    ///<summary>
    /// Representation of RGB colors in 24 bit format
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Color24 : IEquatable<Color24>
    {
        public byte r;
        public byte g;
        public byte b;

        public Color24(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        
        public static implicit operator Color24(Color32 color)
        {
            return new Color24(color.r, color.g, color.b);
        }

        public bool Equals(Color24 other)
        {
            return r == other.r && g == other.g && b == other.b;
        }
    }
}