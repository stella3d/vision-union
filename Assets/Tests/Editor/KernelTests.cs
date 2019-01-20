using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;

namespace VisionUnion.Tests
{
	public class KernelTests
	{
		[TestCase(2, 2)]
		[TestCase(3, 3)]
		[TestCase(5, 5)]
		[TestCase(1, 3)]
		[TestCase(3, 1)]
		public void ConstructBlankFromDimensions(short x, short y)
		{
			var kernel = new Kernel<short>(x, y, Allocator.Temp);
			Assert.IsTrue(kernel.Data.IsCreated);
			Assert.AreEqual(x, kernel.Width);
			Assert.AreEqual(y, kernel.Height);
			Assert.AreEqual(x * y, kernel.Data.Length);
			kernel.Dispose();
		}
		
		[Test]
		public void ContructFrom2dInput()
		{
			var sY = new Kernel<short>(Kernels.Short.Sobel.X, Allocator.Temp);
			sY.Print();
			AssertFlatRepresentationValid(Kernels.Short.Sobel.X, sY.Data);
			sY.Dispose();

			var sX = new Kernel<short>(Kernels.Short.Sobel.Y, Allocator.Temp);
			sX.Print();
			AssertFlatRepresentationValid(Kernels.Short.Sobel.Y, sX.Data);
			sX.Dispose();
		}

		[Test]
		public void ContructFrom1dInput_Horizontal()
		{
			var kernelLength = Kernels.Short.Sobel.yHorizontal.Length;
			var sX = new Kernel<short>(Kernels.Short.Sobel.yHorizontal, true, Allocator.Temp);
			sX.Print();
			Assert.AreEqual(1, sX.Height);
			Assert.AreEqual(kernelLength, sX.Width);
			Assert.AreEqual(kernelLength, sX.Data.Length);
			sX.Dispose();
		}

		[Test]
		public void ContructFrom1dInput_Vertical()
		{
			var kernelLength = Kernels.Short.Sobel.yHorizontal.Length;
			var sY = new Kernel<short>(Kernels.Short.Sobel.yVertical, false, Allocator.Temp);
			sY.Print();
			Assert.AreEqual(1, sY.Width);
			Assert.AreEqual(kernelLength, sY.Height);
			Assert.AreEqual(kernelLength, sY.Data.Length);
			sY.Dispose();
		}
		
		[Test]
		public void BracketAccessorsWork()
		{
			var kernel = new Kernel<short>(3, 3, Allocator.Temp);
			kernel[0, 0] = 10;
			kernel[0, 2] = 20;
			Assert.AreEqual(10, kernel[0, 0]);
			Assert.AreEqual(20, kernel[0, 2]);
			kernel.Dispose();
		}
		
		[Test]
		public void GetRow()
		{
			var kernel = new Kernel<short>(Kernels.Short.Sobel.X, Allocator.Temp);
			var row = kernel.GetRow(1);
			Assert.AreEqual(row[0], Kernels.Short.Sobel.X[1, 0]);
			Assert.AreEqual(row[1], Kernels.Short.Sobel.X[1, 1]);
			Assert.AreEqual(row[2], Kernels.Short.Sobel.X[1, 2]);
			kernel.Dispose();
		}
		
		[TestCaseSource(typeof(KernelBoundsCases), "Square")]
		public void VerifyGetBounds_SquareKernels(int size, Vector2Int nBound, Vector2Int pBound)
		{
			var count = size * size;
			var kernel = new Kernel<short>(size, size, Allocator.Temp);
			Assert.AreEqual(count, kernel.Data.Length);
			
			Debug.Log(kernel.Bounds);
			Assert.AreEqual(nBound, kernel.Bounds.negative);
			Assert.AreEqual(pBound, kernel.Bounds.positive);
			kernel.Dispose();
		}

		[Test]
		public void SeparateKernel()
		{
			var gauss = Kernels.Float.GaussianBlurApproximate3x3;
			var separated = new float[3, 3][];
			gauss.TrySeparate(out separated);
			Assert.AreEqual(3, separated.GetLength(0));
			Assert.AreEqual(3, separated.GetLength(1));
			//Assert.AreEqual(2, separated.GetLength(2));
		}

		public static class KernelBoundsCases
		{
			public static IEnumerable Square
			{
				get
				{
					yield return new TestCaseData(1, Vector2Int.zero, Vector2Int.zero);
					yield return new TestCaseData(2, Vector2Int.zero, Vector2Int.one);
					yield return new TestCaseData(3, Vector2Int.one * -1, Vector2Int.one);
					yield return new TestCaseData(4, Vector2Int.one * -1, Vector2Int.one * 2);
					yield return new TestCaseData(5, Vector2Int.one * -2, Vector2Int.one * 2);
					yield return new TestCaseData(6, Vector2Int.one * -2, Vector2Int.one * 3);
					yield return new TestCaseData(7, Vector2Int.one * -3, Vector2Int.one * 3);
				}
			}
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