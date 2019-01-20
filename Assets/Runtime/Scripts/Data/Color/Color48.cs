using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VisionUnion
{
    ///<summary>
    /// Representation of RGB colors in 48 bit format
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Color48 : IEquatable<Color48>
    {
        public float r;
        public float g;
        public float b;

        public Color48(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        
        public static implicit operator Color48(Color color)
        {
            return new Color48(color.r, color.g, color.b);
        }

        public bool Equals(Color48 other)
        {
            return r.Equals(other.r) && g.Equals(other.g) && b.Equals(other.b);
        }
    }
}