using VisionUnion;
using NUnit.Framework;
using Unity.Collections;

public partial class PoolingTests
{
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
	
	static readonly float[] k_Intensities5x5MeanPooled3x3Stride2x2 = 
	{
		5/3f, 5/3f,	
		10/9f, 4/3f
	};
	
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
	public static void MeanPooling_5x5Input_3x3Kernel_2x2Stride()
	{
		var intensities5x5MeanPooled3x3Stride2x2 = new NativeArray<float>(k_Intensities5x5MeanPooled3x3Stride2x2.Length, Allocator.Temp);

		Pool.Mean(s_Intensities5x5Native, intensities5x5MeanPooled3x3Stride2x2, 5, 5, 3, 3, 2, 2);

		intensities5x5MeanPooled3x3Stride2x2.AssertCollectionEqual(k_Intensities5x5MeanPooled3x3Stride2x2);
		intensities5x5MeanPooled3x3Stride2x2.Dispose();
	}
}
