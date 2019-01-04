using System.Collections;
using System.Collections.Generic;
using System.Text;
using BurstVision;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public static class UtilityTests
{
	static readonly byte[] k_Intensities3x3 = 
	{
		5, 2, 5,
		3, 6, 3,
		5, 2, 5
	};
	
	static readonly byte[] k_Intensities3x3IntegralImage = 
	{
		5, 7, 12,				// row sum = 12 = 5 + 2 + 5
		8, 16, 24,				// row sum = 12 = 6 + 3 + 6, 
		13, 23, 36				// column sum = 36 = 12 + 12 + 12, 
	};
	
	static readonly byte[] k_Intensities5x5 = 
	{
		4, 6, 8, 7, 6,
		3, 4, 7, 8, 7,
		5, 3, 6, 7, 8,
		4, 2, 5, 6, 7,
		3, 2, 4, 5, 7
	};
	
	static StringBuilder s_String = new StringBuilder();
	
	[Test]
	public static void IntegralImage_3x3VerifyOutput()
	{
		var integral3x3 = new int[k_Intensities3x3.Length];
		Operations.IntegralImage(k_Intensities3x3, integral3x3, 3, 3);
		
		DebugUtils.LogFlat2DMatrix(integral3x3, 3, 3);
		
		for (int i = 0; i < integral3x3.Length; i++)
		{
			Assert.AreEqual(k_Intensities3x3IntegralImage[i], integral3x3[i]);
		}
	}
	
	[Test]
	public static void Sobel_FromGrayscale()
	{
		var sobel = new byte[k_Intensities5x5.Length];
		Operations.Sobel(k_Intensities5x5, sobel, 10f, 5, 5);
		
		DebugUtils.LogFlat2DMatrix(sobel, 5, 5);
	}
}
