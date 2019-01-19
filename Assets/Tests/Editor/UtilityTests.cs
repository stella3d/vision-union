using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace VisionUnion.Tests
{
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
			5, 7, 12, // row sum = 12 = 5 + 2 + 5
			8, 16, 24, // row sum = 12 = 6 + 3 + 6, 
			13, 23, 36 // column sum = 36 = 12 + 12 + 12, 
		};

		static readonly byte[] k_Intensities5x5 =
		{
			5, 4, 3, 8, 3,
			3, 9, 1, 2, 6,
			9, 6, 0, 5, 7,
			7, 3, 6, 5, 9,
			1, 2, 2, 8, 3
		};

		static readonly byte[] k_Intensities5x5IntegralImage =
		{
			5, 9, 12, 20, 23,
			8, 21, 25, 35, 44,
			17, 36, 40, 55, 71,
			24, 46, 56, 76, 101,
			25, 49, 61, 89, 117
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
		public static void IntegralImage_5x5VerifyOutput()
		{
			var integral5x5 = new int[k_Intensities5x5.Length];
			Operations.IntegralImage(k_Intensities5x5, integral5x5, 5, 5);

			DebugUtils.LogFlat2DMatrix(integral5x5, 5, 5);

			for (int i = 0; i < integral5x5.Length; i++)
			{
				Assert.AreEqual(k_Intensities5x5IntegralImage[i], integral5x5[i]);
			}
		}

		[Test]
		public static void IntegralImage_Average3x3()
		{
			var integral5x5 = new int[k_Intensities5x5.Length];
			Operations.IntegralImage(k_Intensities5x5, integral5x5, 5, 5);

			DebugUtils.LogFlat2DMatrix(integral5x5, 5, 5);

			var avg5x = new float[k_Intensities5x5.Length];
			Operations.Average3x3(integral5x5, avg5x, 5, 5);

			Debug.Log("averages ???");
			DebugUtils.LogFlat2DMatrix(avg5x, 5, 5);
		}

		[Test]
		public static void Sobel_FromGrayscale()
		{
			var sobel = new byte[k_Intensities5x5.Length];
			Operations.Sobel(k_Intensities5x5, sobel, 10f, 5, 5);

			DebugUtils.LogFlat2DMatrix(sobel, 5, 5);
		}
	}
}