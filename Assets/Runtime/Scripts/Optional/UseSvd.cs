using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Accord.Math.Decompositions;
using VisionUnion;

public static class MatrixOps
{
    public static void DecomposeMatrix(double[,] matrix)
    {
        var svd = new SingularValueDecomposition(matrix);
        Debug.Log("rank: " + svd.Rank);
        Debug.Log("diagonal matrix:\n" + svd.DiagonalMatrix.MatrixToString());
        Debug.Log("right singular vectors matrix:\n" + svd.RightSingularVectors.MatrixToString());
        Debug.Log("left singular vector matrix:\n" + svd.LeftSingularVectors.MatrixToString());
    }
}
