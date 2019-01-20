using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion
{
    [BurstCompile]
    public struct IntegralImageFromGrayscaleJob : IJob
    {
        public int width;
        public int height;

        [ReadOnly] public NativeArray<float> GrayscaleTexture;

        public NativeArray<float> IntegralTexture;

        public void Execute()
        {
            var width = this.width;

            // set the top left pixel by itself so we don't have to branch during iteration.
            // since this is the first pixel, the output and input are the same
            IntegralTexture[0] = GrayscaleTexture[0];

            // do the rest of the top row 
            var previousSum = 0f;
            for (var w = 1; w < width; w++)
            {
                var localIntensity = GrayscaleTexture[w];
                var summedIntensity = localIntensity + previousSum;
                previousSum = summedIntensity;
                IntegralTexture[w] = summedIntensity;
            }

            for (var h = 1; h < height; h++)
            {
                var yIndex = h * width;
                var firstLocalIntensity = GrayscaleTexture[yIndex];

                var firstTopIndex = (h - 1) * width;
                var firstTopSumIntensity = IntegralTexture[firstTopIndex];

                var firstSum = firstLocalIntensity + firstTopSumIntensity;
                IntegralTexture[yIndex] = firstSum;

                for (int w = 1; w < width; w++)
                {
                    var index = yIndex + w;

                    var localIntensity = GrayscaleTexture[index];

                    var leftIndex = index - 1;
                    var topIndex = firstTopIndex + w;
                    var topLeftIndex = topIndex - 1;

                    var leftSumIntensity = IntegralTexture[leftIndex];
                    var topSumIntensity = IntegralTexture[topIndex];
                    var topLeftSumIntensity = IntegralTexture[topLeftIndex];

                    var summedIntensity = localIntensity + leftSumIntensity + topSumIntensity - topLeftSumIntensity;
                    IntegralTexture[index] = summedIntensity;
                }
            }
        }
    }

    [BurstCompile]
    public struct IntegralImageFromGrayscaleByteJob : IJob
    {
        public int width;
        public int height;

        [ReadOnly] public NativeArray<byte> GrayscaleTexture;

        public NativeArray<int> IntegralTexture;

        public void Execute()
        {
            var width = this.width;

            // set the top left pixel by itself so we don't have to branch during iteration.
            // since this is the first pixel, the output and input are the same
            IntegralTexture[0] = GrayscaleTexture[0];

            // do the rest of the top row 
            var previousSum = 0;
            for (var w = 1; w < width; w++)
            {
                var localIntensity = GrayscaleTexture[w];
                var summedIntensity = localIntensity + previousSum;
                previousSum = summedIntensity;
                IntegralTexture[w] = summedIntensity;
            }

            for (var h = 1; h < height; h++)
            {
                var yIndex = h * width;
                var firstLocalIntensity = GrayscaleTexture[yIndex];

                var firstTopIndex = yIndex - width;
                var firstTopSumIntensity = IntegralTexture[firstTopIndex];

                var firstSum = firstLocalIntensity + firstTopSumIntensity;
                IntegralTexture[yIndex] = firstSum;

                for (var w = 1; w < width; w++)
                {
                    var index = yIndex + w;

                    var localIntensity = GrayscaleTexture[index];

                    var leftIndex = index - 1;
                    var topIndex = firstTopIndex + w;
                    var topLeftIndex = topIndex - 1;

                    var leftSumIntensity = IntegralTexture[leftIndex];
                    var topSumIntensity = IntegralTexture[topIndex];
                    var topLeftSumIntensity = IntegralTexture[topLeftIndex];

                    var summedIntensity = localIntensity + leftSumIntensity + topSumIntensity - topLeftSumIntensity;
                    IntegralTexture[index] = summedIntensity;
                }
            }
        }
    }
}