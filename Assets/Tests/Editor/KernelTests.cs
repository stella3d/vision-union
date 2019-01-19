﻿using NUnit.Framework;
using Unity.Collections;

namespace VisionUnion.Tests
{
	public class KernelTests
	{
		[Test]
		public void ContructFrom2dInput()
		{
			var sY = new Kernel<short>(Kernels.Sobel.X, Allocator.Temp);
			sY.Print();
			AssertFlatRepresentationValid(Kernels.Sobel.X, sY.Data);
			sY.Dispose();

			var sX = new Kernel<short>(Kernels.Sobel.Y, Allocator.Temp);
			sX.Print();
			AssertFlatRepresentationValid(Kernels.Sobel.Y, sX.Data);
			sX.Dispose();
		}

		[Test]
		public void ContructFrom1dInput_Horizontal()
		{
			var kernelLength = Kernels.Sobel.yHorizontal.Length;
			var sX = new Kernel<short>(Kernels.Sobel.yHorizontal, true, Allocator.Temp);
			sX.Print();
			Assert.AreEqual(1, sX.Height);
			Assert.AreEqual(kernelLength, sX.Width);
			Assert.AreEqual(kernelLength, sX.Data.Length);
			sX.Dispose();
		}

		[Test]
		public void ContructFrom1dInput_Vertical()
		{
			var kernelLength = Kernels.Sobel.yHorizontal.Length;
			var sY = new Kernel<short>(Kernels.Sobel.yVertical, false, Allocator.Temp);
			sY.Print();
			Assert.AreEqual(1, sY.Width);
			Assert.AreEqual(kernelLength, sY.Height);
			Assert.AreEqual(kernelLength, sY.Data.Length);
			sY.Dispose();
		}

		void AssertFlatRepresentationValid<T>(T[,] input2D, NativeArray<T> flat)
			where T : struct
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
}