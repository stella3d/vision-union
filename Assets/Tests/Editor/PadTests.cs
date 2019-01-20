using NUnit.Framework;
using Unity.Collections;

namespace VisionUnion.Tests
{
	public class PadTests
	{
		NativeArray<byte> m_Input5x5;
		NativeArray<byte> m_Input6x6;

		[OneTimeSetUp]
		public void BeforeAll()
		{
			m_Input5x5 = new NativeArray<byte>(InputImages.Byte5x5, Allocator.Persistent);
			m_Input6x6 = new NativeArray<byte>(InputImages.Byte6x6, Allocator.Persistent);
		}

		[OneTimeTearDown]
		public void AfterAll()
		{
			m_Input5x5.Dispose();
			m_Input6x6.Dispose();
		}

		[Test]
		public void BasicPad()
		{
			
		}
	}
}