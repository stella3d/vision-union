using System.Collections;
using System.Collections.Generic;
using System.Text;
using BurstVision;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class PoolingTests
{
	static readonly byte[] k_Intensities5x5 = 
	{
		3, 3, 2, 1, 0,	
		0, 0, 1, 3, 1,
		3, 1, 2, 2, 3,
		2, 0, 0, 2, 2,
		2, 0, 0, 0, 1
	};
	
	static readonly float[] k_Intensities5x5MeanPooled2x2 = 
	{
		1.5f, 1.5f, 7/4f, 5/4f,
		1.0f, 1.0f, 2.0f, 9/4f,
		1.5f, 0.75f, 1.5f, 9/4f,
		1.0f, 0.0f, 0.5f, 1.25f,
	};
	
	static readonly float[] k_Intensities5x5MeanPooled3x3 = 
	{
		5/3f, 5/3f, 5/3f,
		1.0f, 11/9f, 16/9f,
		10/9f, 7/9f, 12/9f
	};
	
	static readonly byte[] k_Intensities5x5MaxPooled3x3 = 
	{
		3, 3, 3,	
		3, 3, 3,
		3, 2, 3
	};
	
	NativeArray<float> m_MeanPool;
	static NativeArray<byte> s_Intensities5x5Native;

	[OneTimeSetUp]
	public void BeforeAll()
	{
		s_Intensities5x5Native = new NativeArray<byte>(k_Intensities5x5, Allocator.Temp);
	}

	[Test]
	public static void MeanPooling_2x2Kernel_5x5Input()
	{
		var intensities5x5MeanPooled2x2 = new NativeArray<float>(k_Intensities5x5MeanPooled2x2.Length, Allocator.Temp);

		Operations.MeanPool(s_Intensities5x5Native, intensities5x5MeanPooled2x2, 5, 5, 2, 2, 1, 1);

		intensities5x5MeanPooled2x2.AssertCollectionEqual(k_Intensities5x5MeanPooled2x2);
		intensities5x5MeanPooled2x2.Dispose();
	}
	
	[Test]
	public static void MeanPooling_3x3Kernel_5x5Input()
	{
		var intensities5x5MeanPooled3x3 = new NativeArray<float>(k_Intensities5x5MeanPooled3x3.Length, Allocator.Temp);

		Operations.MeanPool(s_Intensities5x5Native, intensities5x5MeanPooled3x3, 5, 5, 3, 3, 1, 1);

		intensities5x5MeanPooled3x3.AssertApproximatelyEqual(k_Intensities5x5MeanPooled3x3);
		intensities5x5MeanPooled3x3.Dispose();
	}
	
	[Test]
	public static void MaxPooling_5x5Input_3x3Kernel_1x1Stride()
	{
		var intensities5x5MaxPooled3x3 = new NativeArray<byte>(k_Intensities5x5MaxPooled3x3.Length, Allocator.Temp);

		Operations.MaxPool(s_Intensities5x5Native, intensities5x5MaxPooled3x3, 5, 5, 3, 3, 1, 1);

		intensities5x5MaxPooled3x3.AssertCollectionEqual(k_Intensities5x5MaxPooled3x3);
		intensities5x5MaxPooled3x3.Dispose();
	}
}
