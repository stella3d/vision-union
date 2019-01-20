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
        foreach (var d in svd.Diagonal)
        {
            Debug.Log(d);
        }
    }
}
