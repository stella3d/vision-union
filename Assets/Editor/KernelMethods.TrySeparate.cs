using Accord.Math;
using Accord.Math.Decompositions;
using Unity.Mathematics;

namespace VisionUnion
{
    // If providing manually separated kernels is all you need, you can
    // delete Accord.Math and define NO_ACCORD in the project settings.
    // These methods will simply do nothing.
    public static class EditorKernelMethods
    {
#if NO_ACCORD     
        internal static bool TrySeparate(float[,] kernel, out float[][] separated)
        {
            separated = null;
            return false;
        }
        
        internal static bool TrySeparate(short[,] kernel, out short[][] separated)
        {
            separated = null;
            return false;
        }
        
        internal static bool TrySeparate(double[,] kernel, out double[][] separated)
        {
            separated = null;
            return false;
        }
#else
        // this approach for separating kernels is adapted from this very helpful post
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
            separated[0] = column;
            separated[1] = row;
            return true;
        }
        
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
        
        public static bool TrySeparate(this short[,] kernel, out short[][] separated)
        {
            var matrix = kernel.ToDouble();
            var output = new double[kernel.GetLength(0)][];
            TrySeparate(matrix, out output);
            
            separated = new short[kernel.GetLength(0)][];
            separated[0] = output[0].ToShort();
            separated[1] = output[1].ToShort();
            return true;
        }
#endif
    }
}