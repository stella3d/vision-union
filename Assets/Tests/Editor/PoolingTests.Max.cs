using System.Collections;
using System.Collections.Generic;
using System.Text;
using BurstVision;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public partial class PoolingTests
{
	static readonly byte[] k_Bytes6x6 = 
	{
		3, 4, 2, 1, 5, 3,	
		0, 2, 1, 3, 1, 2,
		3, 1, 2, 2, 3, 5,
		2, 3, 0, 2, 2, 6,
		1, 0, 3, 0, 1, 4,
		4, 1, 5, 2, 2, 3
	};
	
	static readonly byte[] k_Intensities5x5 = 
	{
		3, 3, 2, 1, 0,	
		0, 0, 1, 3, 1,
		3, 1, 2, 2, 3,
		2, 0, 0, 2, 2,
		2, 0, 0, 0, 1
	};
	
	static readonly byte[] k_Intensities5x5MaxPooled2x2 = 
	{
		3, 3, 3, 3,	
		3, 2, 3, 3,
		3, 2, 2, 3,
		2, 0, 2, 2
	};

	static readonly byte[] k_Intensities5x5MaxPooled3x3 = 
	{
		3, 3, 3,	
		3, 3, 3,
		3, 2, 3
	};
	
	static readonly byte[] k_Intensities5x5MaxPooled3x3Stride2x2 = 
	{
		3, 3,	
		3, 3
	};
	
	NativeArray<float> m_MeanPool;
	static NativeArray<byte> s_Intensities5x5Native;

	[OneTimeSetUp]
	public void BeforeAll()
	{
		s_Intensities5x5Native = new NativeArray<byte>(k_Intensities5x5, Allocator.Persistent);
	}
	
	[OneTimeTearDown]
	public void AfterAll()
	{
		s_Intensities5x5Native.Dispose();
	}
	
	[TestCaseSource(typeof(PoolCases), "FiveBy")]
	public static void MaxPooling_5x5Input_2x2Kernel_1x1Stride(int size, int kernel, int stride, byte[] source)
	{
		var pooled = new NativeArray<byte>(source.Length, Allocator.Temp);

		Pool.Max(s_Intensities5x5Native, pooled, size, size, kernel, kernel, stride, stride);

		pooled.AssertCollectionEqual(source);
		pooled.Dispose();
	}

	public static class PoolCases
	{
		public static IEnumerable FiveBy
		{
			get
			{
				yield return new TestCaseData(5, 2, 1, k_Intensities5x5MaxPooled2x2);
				yield return new TestCaseData(5, 3, 1, k_Intensities5x5MaxPooled3x3);
				yield return new TestCaseData(5, 3, 2, k_Intensities5x5MaxPooled3x3Stride2x2);
			}
		}
	}
}
