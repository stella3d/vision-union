using Accord.Math;
using Accord.Math.Decompositions;
using Unity.Mathematics;
using UnityEngine;

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
        
        // this approach for separating kernels is adapted from this helpful post
        // https://blogs.mathworks.com/steve/2006/11/28/separable-convolution-part-2
        public static bool TrySeparate(this double[,] kernel, out double[][] separated)
        {
            var svd = new SingularValueDecomposition(kernel);
            // any separable kernel has a rank of 1
            if (svd.Rank != 1)
            {
                separated = null;
                return false;
            }

            var scaleFactor = math.sqrt(svd.Diagonal[0]);
            var column = svd.LeftSingularVectors.GetColumn(0).Multiply(scaleFactor);
            var row = svd.RightSingularVectors.GetColumn(0).Multiply(scaleFactor);

            separated = new double[kernel.GetLength(0)][];

            var mult = column.MultiplyWithRow(row);
            Debug.Log("resulting matrix:\n" + mult.MatrixToString());
            
            separated[0] = column;
            separated[1] = row;
            return true;
        }
    }
}