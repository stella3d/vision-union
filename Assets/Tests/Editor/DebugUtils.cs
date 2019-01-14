using System.Collections.Generic;
using System.Text;
using BurstVision;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;

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
    
    public static void AssertApproximatelyEqual(this NativeArray<float> native, float[] managed) 
    {
        Assert.AreEqual(native.Length, managed.Length);
        for (var i = 0; i < native.Length; i++)
        {
            Assert.AreApproximatelyEqual(native[i], managed[i]);
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
}
