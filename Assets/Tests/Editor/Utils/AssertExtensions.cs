using Unity.Collections;
using UnityEngine.Assertions;

namespace VisionUnion.Tests
{
    public static class AssertExtensions
    {
        public static void AssertCollectionEqual<T>(this NativeArray<T> native, T[] managed) 
            where T: struct
        {
            Assert.AreEqual(native.Length, managed.Length);
            for (var i = 0; i < native.Length; i++)
            {
                Assert.AreEqual(native[i], managed[i]);
            }
        }
        
        public static void AssertDeepEqual<T>(this NativeArray<T> self, NativeArray<T> other) 
            where T: struct
        {
            Assert.AreEqual(self.Length, other.Length);
            for (var i = 0; i < self.Length; i++)
            {
                Assert.AreEqual(self[i], other[i]);
            }
        }
        
        public static void AssertDeepEqual(this NativeArray<byte> bytes, NativeArray<short> shorts) 
        {
            Assert.AreEqual(bytes.Length, shorts.Length);
            for (var i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(bytes[i], shorts[i]);
            }
        }
        
        public static void AssertApproximatelyEqual(this NativeArray<float> native, float[] managed) 
        {
            Assert.AreEqual(native.Length, managed.Length);
            for (var i = 0; i < native.Length; i++)
            {
                Assert.AreApproximatelyEqual(native[i], managed[i]);
            }
        }
        
        public static void AssertEqualWithin(this ImageData<float> a, ImageData<float> b, float tolerance = 0.01f) 
        {
            Assert.AreEqual(a.Buffer.Length, b.Buffer.Length);
            for (var i = 0; i < a.Buffer.Length; i++)
            {
                Assert.AreApproximatelyEqual(a.Buffer[i], b.Buffer[i], tolerance);
            }
        }
    }
}