using System;
using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace  VisionUnion.Tests
{
    public static class DebugUtils
    {
        static StringBuilder s_String = new StringBuilder();
    
        public static void LogFlat2DMatrix<T>(T[] matrix, int width, int height)
        {
            s_String.Length = 0;
            for (int y = 0; y < height; y++)
            {
                var rowIndex = y * width;
                for (int x = 0; x < width - 1; x++)
                {
                    s_String.AppendFormat("{0}, ", matrix[rowIndex + x]);
                }
    
                s_String.AppendLine(matrix[rowIndex + width - 1].ToString());    // end of row, no comma
            }
            
            Debug.Log(s_String);
        }
        
        public static void LogFlat2DMatrix<T>(NativeArray<T> matrix, int width, int height)
            where T: struct
        {
            s_String.Length = 0;
            for (int y = 0; y < height; y++)
            {
                var rowIndex = y * width;
                for (int x = 0; x < width - 1; x++)
                {
                    s_String.AppendFormat("{0}, ", matrix[rowIndex + x]);
                }
    
                s_String.AppendLine(matrix[rowIndex + width - 1].ToString());    // end of row, no comma
            }
            
            Debug.Log(s_String);
        }
        
        public static void AssertCollectionEqual<T>(this NativeArray<T> native, T[] managed) 
            where T: struct
        {
            Assert.AreEqual(native.Length, managed.Length);
            for (var i = 0; i < native.Length; i++)
            {
                Assert.AreEqual(native[i], managed[i]);
            }
        }
        
        public static void AssertDeepEqual<T>(this NativeArray<T> self, NativeArray<T> other) 
            where T: struct
        {
            Assert.AreEqual(self.Length, other.Length);
            for (var i = 0; i < self.Length; i++)
            {
                Assert.AreEqual(self[i], other[i]);
            }
        }
        
        public static void AssertDeepEqual(this NativeArray<byte> bytes, NativeArray<short> shorts) 
        {
            Assert.AreEqual(bytes.Length, shorts.Length);
            for (var i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(bytes[i], shorts[i]);
            }
        }
        
        public static void AssertApproximatelyEqual(this NativeArray<float> native, float[] managed) 
        {
            Assert.AreEqual(native.Length, managed.Length);
            for (var i = 0; i < native.Length; i++)
            {
                Assert.AreApproximatelyEqual(native[i], managed[i]);
            }
        }
        
        public static void AssertEqualWithin(ImageData<float> a, ImageData<float> b, float tolerance = 0.01f) 
        {
            Assert.AreEqual(a.Buffer.Length, b.Buffer.Length);
            for (var i = 0; i < a.Buffer.Length; i++)
            {
                Assert.AreApproximatelyEqual(a.Buffer[i], b.Buffer[i], tolerance);
            }
        }
        
        public static void Print<T>(this Kernel<T> kernel) 
            where T: struct
        {
            s_String.Length = 0;
            var matrix = kernel.Data;
            for (int y = 0; y < kernel.Height; y++)
            {
                var rowIndex = y * kernel.Width;
                for (int x = 0; x < kernel.Width - 1; x++)
                {
                    s_String.AppendFormat("{0}, ", matrix[rowIndex + x]);
                }
    
                s_String.AppendLine(matrix[rowIndex + kernel.Width - 1].ToString());    // end of row, no comma
            }
            
            Debug.Log(s_String);
        }
        
        public static void Print(this ImageData<byte> image) 
        {
            if (image.Height > 12 || image.Width > 12)
            {
                Debug.LogWarning("Printing is only suitable for images 12x12 and under!");
                return;
            }

            s_String.Length = 0;
            var buffer = image.Buffer;
            for (var y = 0; y < image.Height; y++)
            {
                var rowIndex = y * image.Width;
                for (var x = 0; x < image.Width - 1; x++)
                {
                    s_String.AppendFormat("{0}, ", buffer[rowIndex + x]);
                }
    
                s_String.AppendLine(((short)buffer[rowIndex + image.Width - 1]).ToString());    
            }
            
            Debug.Log(s_String);
        }

        
        public static void Print(this ImageData<float> image, string decimalFormat = "F2") 
        {
            if (image.Height > 8 || image.Width > 8)
            {
                Debug.LogWarning("Printing is only suitable for images 8x8 and under!");
                return;
            }

            s_String.Length = 0;
            var buffer = image.Buffer;
            for (var y = 0; y < image.Height; y++)
            {
                var rowIndex = y * image.Width;
                for (var x = 0; x < image.Width - 1; x++)
                {
                    s_String.AppendFormat("{0}, ", buffer[rowIndex + x].ToString(decimalFormat));
                }
    
                s_String.AppendLine(buffer[rowIndex + image.Width - 1].ToString(decimalFormat));    
            }
            
            Debug.Log(s_String);
        }

        public static Texture2D NewFilledTexture<T>(int width, int height, T value, TextureFormat format)
            where T: struct
        {
            var texture = new Texture2D(width, height, format, false);
            var data = texture.GetRawTextureData<T>();
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = value;
            }

            return texture;
        }
    }
}