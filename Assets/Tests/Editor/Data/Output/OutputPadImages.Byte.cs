using System.Collections;
using NUnit.Framework;

namespace VisionUnion.Tests
{
    internal static partial class OutputPadImages
    {
        internal static readonly byte[] Input5x5ZeroPadUniform1 =
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 9, 9, 6, 2, 1, 0,
            0, 7, 8, 7, 3, 2, 0,
            0, 6, 7, 7, 5, 3, 0,
            0, 5, 6, 6, 6, 4, 0,
            0, 3, 5, 5, 7, 5, 0,
            0, 0, 0, 0, 0, 0, 0
        };
        
        internal static readonly byte[] Input5x5ZeroPadUniform2 =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 9, 9, 6, 2, 1, 0, 0,
            0, 0, 7, 8, 7, 3, 2, 0, 0,
            0, 0, 6, 7, 7, 5, 3, 0, 0,
            0, 0, 5, 6, 6, 6, 4, 0, 0,
            0, 0, 3, 5, 5, 7, 5, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0
        };
        
        internal static readonly byte[] Input5x5ZeroPad1x1x2x2 =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 9, 9, 6, 2, 1, 0, 0,
            0, 0, 7, 8, 7, 3, 2, 0, 0,
            0, 0, 6, 7, 7, 5, 3, 0, 0,
            0, 0, 5, 6, 6, 6, 4, 0, 0,
            0, 0, 3, 5, 5, 7, 5, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0
        };
        
        internal static readonly byte[] Input5x5ZeroPad2x2x0x0 =
        {
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            9, 9, 6, 2, 1,
            7, 8, 7, 3, 2,
            6, 7, 7, 5, 3,
            5, 6, 6, 6, 4,
            3, 5, 5, 7, 5,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0
        };
        
        public static IEnumerable ConstantCases
        {
            get
            {
                var inputImageData = new ImageData<byte>(InputImages.Byte5x5, 5, 5);
                
                yield return new TestCaseData(inputImageData, 
                    new Padding(1), 
                    new ImageData<byte>(Input5x5ZeroPadUniform1, 7, 7));

                yield return new TestCaseData(inputImageData, 
                    new Padding(1, 1, 2, 2), 
                    new ImageData<byte>(Input5x5ZeroPad1x1x2x2, 9, 7));
                
                yield return new TestCaseData(inputImageData, 
                    new Padding(2, 2, 0, 0), 
                    new ImageData<byte>(Input5x5ZeroPad2x2x0x0, 5, 9));
            }
        }
    }
}