using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace VisionUnion
{
    public struct PadData
    {
        public int top;
        public int bottom;
        public int left;
        public int right;
    }

    public static class Pad
    {
        public static ImageData<T> Image<T>(ImageData<T> input, PadData padding, PadMode mode, 
            T constantValue = default(T), Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            var output = default(ImageData<T>);
            switch (mode)
            {
                case PadMode.Constant:
                    output = Constant(input, padding, constantValue, allocator);
                    break;
            }

            return output;
        }
        
        public static ImageData<T> Constant<T>(ImageData<T> input, PadData pad, T constantValue = default(T), 
            Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            var oldLength = input.Buffer.Length;
            var newWidth = input.Width + pad.left + pad.right;
            var newHeight = input.Height + pad.top + pad.bottom;
            var newLength = newWidth * newHeight;

            var newBuffer = new NativeArray<T>(newLength, allocator);
            return new ImageData<T>(newBuffer, newLength, newHeight);
        }
    }
}