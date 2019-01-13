using System.Collections;
using System.Collections.Generic;
using System.Text;
using BurstVision;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class KernelTests
{
	
	
	[Test]
	public void ContructFrom2dInput()
	{
		var sY = new Kernel(ShortKernels.Sobel.Vertical, Allocator.Temp);
		sY.Print();
		AssertFlatRepresentationValid(ShortKernels.Sobel.Vertical, sY.Data);
		sY.Dispose();
		
		var sX = new Kernel(ShortKernels.Sobel.Horizontal, Allocator.Temp);
		sX.Print();
		AssertFlatRepresentationValid(ShortKernels.Sobel.Horizontal, sX.Data);
		sX.Dispose();
	}


	void AssertFlatRepresentationValid<T>(T[,] input2D, NativeArray<T> flat) 
		where T: struct
	{
		var width = input2D.GetLength(0);
		var height = input2D.GetLength(1);
		Assert.AreEqual(width * height, flat.Length);
            
		var flatIndex = 0;
		for (var y = 0; y < height; y++)
		{
			for (var x = 0; x < width; x++)
			{
				Assert.AreEqual(input2D[y, x], flat[flatIndex]);
				flatIndex++;
			}
		}
	}
}
