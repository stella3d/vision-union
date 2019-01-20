using NUnit.Framework;

namespace VisionUnion.Tests
{
	public class PadTests
	{
		ImageData<byte> m_Image5x5;
		ImageData<byte> m_Image6x6;

		Padding m_UniformPad1;
		Padding m_Pad1x1x2x2;
		Padding m_Pad2x2x0x0;

		[OneTimeSetUp]
		public void BeforeAll()
		{
			m_Image5x5 = new ImageData<byte>(InputImages.Byte5x5, 5, 5);
			m_Image6x6 = new ImageData<byte>(InputImages.Byte6x6, 6, 6);
			
			m_UniformPad1 = new Padding(1);
			m_Pad1x1x2x2 = new Padding(1, 1, 2, 2);
			m_Pad2x2x0x0 = new Padding(2, 2, 0, 0);
		}

		[OneTimeTearDown]
		public void AfterAll()
		{
			m_Image5x5.Dispose();
			m_Image6x6.Dispose();
		}

		[TestCaseSource(typeof(OutputPadImages), "ConstantCases")]
		public void ConstantPaddingCases(ImageData<byte> input, Padding pad, ImageData<byte> expected)
		{
			var output = Pad.Constant(input, pad);
			output.Print();
			output.Buffer.AssertDeepEqual(expected.Buffer);
			output.Dispose();
		}
	}
}