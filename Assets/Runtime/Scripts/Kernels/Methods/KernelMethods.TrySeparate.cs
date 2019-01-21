using System;
using System.Linq;
using Accord;
using Accord.Math;
using Accord.Math.Decompositions;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace VisionUnion
{
    public delegate bool TrySeparateShortDelegate(short[,] kernel, out short[][] separated);
    public delegate bool TrySeparateFloatDelegate(float[,] kernel, out float[][] separated);
    public delegate bool TrySeparateDoubleDelegate(double[,] kernel, out double[][] separated);
    
    public static partial class KernelMethods
    {
        public static TrySeparateShortDelegate TrySeparateShort = TrySeparateShortNoop;
        public static TrySeparateFloatDelegate TrySeparateFloat = TrySeparateFloatNoop;
        public static TrySeparateDoubleDelegate TrySeparateDouble = TrySeparateDoubleNoop;

        internal static bool TrySeparateFloatNoop(float[,] kernel, out float[][] separated)
        {
            separated = null;
            return false;
        }
        
        internal static bool TrySeparateShortNoop(short[,] kernel, out short[][] separated)
        {
            separated = null;
            return false;
        }
        
        internal static bool TrySeparateDoubleNoop(double[,] kernel, out double[][] separated)
        {
            separated = null;
            return false;
        }
    }
    
    public static class KernelMethodImplementations
    {
        public static bool TrySeparate(this float[,] kernel, out float[][] separated)
        {
            var matrix = kernel.ToDouble();
            var output = new double[kernel.GetLength(0)][];
            TrySeparate(matrix, out output);
            
            separated = new float[kernel.GetLength(0)][];
            separated[0] = output[0].ToFloat();
            separated[1] = output[1].ToFloat();
            return true;
        }
        
        public static bool TrySeparate(this double[,] kernel, out double[][] separated)
        {
            var matrix = kernel;
            Debug.Log("kernel\n" + matrix.MatrixToString());

            var svd = new SingularValueDecomposition(matrix);
            Debug.Log("rank: " + svd.Rank);
            // any separable kernel has a rank of 1
            if (svd.Rank != 1)
            {
                separated = null;
                return false;
            }

            Debug.Log("right singular vectors matrix:\n" + svd.RightSingularVectors.MatrixToString());
            Debug.Log("left singular vector matrix:\n" + svd.LeftSingularVectors.MatrixToString());

            var scaleFactor = math.sqrt(svd.Diagonal[0]);
            Debug.Log("scalefactor: " + scaleFactor);

            var column = svd.LeftSingularVectors.GetColumn(0).Multiply(scaleFactor);
            var row = svd.RightSingularVectors.GetColumn(0).Multiply(scaleFactor);

            var output = new double[kernel.GetLength(0)][];
            
            Debug.Log("multiplied V?? column:\n" + column.ToColumnString());
            Debug.Log("multiplied U?? row:\n\n" + row.ToRowString());

            var mult = column.MultiplyWithRow(row);
            Debug.Log("resulting matrix:\n" + mult.MatrixToString());

            output[0] = column;
            output[1] = row;
            separated = output;
            return true;
        }
    }
}