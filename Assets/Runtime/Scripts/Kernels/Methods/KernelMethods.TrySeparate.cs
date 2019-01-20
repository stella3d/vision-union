using System;
using Accord.Math;
using Accord.Math.Decompositions;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace VisionUnion
{
    public delegate bool TrySeparateShortDelegate(float[,] kernel, out float[,] separated);
    public delegate bool TrySeparateFloatDelegate(float[,] kernel, out float[,] separated);
    public delegate bool TrySeparateDoubleDelegate(float[,] kernel, out float[,] separated);
    
    public static partial class KernelMethods
    {
        public static TrySeparateShortDelegate TrySeparateShort;
        public static TrySeparateShortDelegate TrySeparateFloat;
        public static TrySeparateShortDelegate TrySeparateDouble;

        internal static bool TrySeparateFloatNoop(float[,] kernel, out float[,] separated)
        {
            separated = null;
            return false;
        }
        
        internal static bool TrySeparateShortNoop(short[,] kernel, out short[,] separated)
        {
            separated = null;
            return false;
        }
        
        internal static bool TrySeparateDoubleNoop(double[,] kernel, out double[,] separated)
        {
            separated = null;
            return false;
        }
    }
    
    public static class KernelMethodImplementations
    {
        public static bool TrySeparate(this float[,] kernel, out float[,][] separated)
        {
            var matrix = kernel.ToDouble();
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

            var scaleFactor = svd.Diagonal[0];
            Debug.Log("scalefactor: " + scaleFactor);

            var factorSqrt = math.sqrt(scaleFactor);

            var firstColumn = svd.LeftSingularVectors.GetColumn(0);
            var multipliedFirstV = firstColumn.Multiply(factorSqrt);

            var firstRow = svd.RightSingularVectors.GetColumn(0);
            var multipliedFirstRow = firstRow.Multiply(factorSqrt);

            var output = new float[kernel.GetLength(0),kernel.GetLength(1)][];
            
            Debug.Log("multiplied V?? column:\n" + multipliedFirstV.ToColumnString());
            Debug.Log("multiplied U?? row:\n\n" + multipliedFirstRow.ToRowString());


            var mult = multipliedFirstV.MultiplyWithRow(multipliedFirstRow);
            Debug.Log("resulting matrix:\n" + mult.MatrixToString());

            
            separated = output;
            return true;
        }
        
        public static bool TrySeparate(this Kernel<float> kernel, out Kernel<float>[] separated)
        {
            var matrix = kernel.ToMatrix().ToDouble();
            Debug.Log("kernel\n" + matrix.MatrixToString());

            var svd = new SingularValueDecomposition(matrix);
            Debug.Log("rank: " + svd.Rank);
            Debug.Log("diagonal matrix:\n" + svd.DiagonalMatrix.MatrixToString());
            Debug.Log("right singular vectors matrix:\n" + svd.RightSingularVectors.MatrixToString());
            Debug.Log("left singular vector matrix:\n" + svd.LeftSingularVectors.MatrixToString());

            var scaleFactor = svd.Diagonal[0];
            Debug.Log("scalefactor: " + scaleFactor);

            var factorSqrt = math.sqrt(scaleFactor);

            var firstColumn = svd.LeftSingularVectors.GetColumn(0);
            var multipliedFirstV = firstColumn.Multiply(factorSqrt);
            
            var multiplied = svd.LeftSingularVectors
                .Multiply(svd.RightSingularVectors).Multiply(scaleFactor);
            
            Debug.Log("multiplied column:\n" + multipliedFirstV.ToColumnString());
            
            separated = null;
            return false;
        }
    }
}