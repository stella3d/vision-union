using System.Collections;
using NUnit.Framework;
using Unity.Collections;

namespace VisionUnion.Tests
{
	public partial class PadTests
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

		[Test]
		public static void BasicPad()
		{
			
		}
	}
}