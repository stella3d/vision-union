using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VisionUnion
{
    ///<summary>
    /// Representation of RGB colors in floating point format
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Color96 : IEquatable<Color96>
    {
        public float r;
        public float g;
        public float b;

        public Color96(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        
        public static implicit operator Color96(Color color)
        {
            return new Color96(color.r, color.g, color.b);
        }
        
        public static implicit operator Color(Color96 color)
        {
            return new Color(color.r, color.g, color.b, 1f);
        }

        public bool Equals(Color96 other)
        {
            return r.Equals(other.r) && g.Equals(other.g) && b.Equals(other.b);
        }
        
        public override string ToString()
        {
            return $"r: {r:F4}, g: {g:F4}, b: {b:F4}";
        }
    }
}