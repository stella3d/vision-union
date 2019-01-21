using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace VisionUnion
{
    [BurstCompile]
    public struct IntegralImageFromGrayscaleJob : IJob
    {
        [ReadOnly] 
        public ImageData<float> Input;
        
        public ImageData<float> Output;
        
        public void Execute()
        {
            var inBuffer = Input.Buffer;
            var outBuffer = Output.Buffer;
            var width = Input.Width;

            // set the top left pixel by itself so we don't have to branch during iteration.
            // since this is the first pixel, the output and input are the same
            outBuffer[0] = inBuffer[0];

            // do the rest of the top row 
            var previousSum = 0f;
            for (var w = 1; w < width; w++)
            {
                var localIntensity = inBuffer[w];
                var summedIntensity = localIntensity + previousSum;
                previousSum = summedIntensity;
                outBuffer[w] = summedIntensity;
            }

            for (var h = 1; h < Input.Height; h++)
            {
                var yIndex = h * width;
                var firstLocalIntensity = inBuffer[yIndex];

                var firstTopIndex = (h - 1) * width;
                var firstTopSumIntensity = outBuffer[firstTopIndex];

                var firstSum = firstLocalIntensity + firstTopSumIntensity;
                outBuffer[yIndex] = firstSum;

                for (var w = 1; w < width; w++)
                {
                    var index = yIndex + w;

                    var localIntensity = inBuffer[index];

                    var leftIndex = index - 1;
                    var topIndex = firstTopIndex + w;
                    var topLeftIndex = topIndex - 1;

                    var leftSumIntensity = outBuffer[leftIndex];
                    var topSumIntensity = outBuffer[topIndex];
                    var topLeftSumIntensity = outBuffer[topLeftIndex];

                    var summedIntensity = localIntensity + leftSumIntensity + topSumIntensity - topLeftSumIntensity;
                    outBuffer[index] = summedIntensity;
                }
            }
        }
    }

    [BurstCompile]
    public struct IntegralImageFromGrayscaleByteJob : IJob
    {
        [ReadOnly] 
        public ImageData<byte> Input;
        
        public ImageData<int> Output;

        public void Execute()
        {
            var inBuffer = Input.Buffer;
            var outBuffer = Output.Buffer;
            var width = Input.Width;

            // set the top left pixel by itself so we don't have to branch during iteration.
            // since this is the first pixel, the output and input are the same
            outBuffer[0] = inBuffer[0];

            // do the rest of the top row 
            var previousSum = 0;
            for (var w = 1; w < width; w++)
            {
                var localIntensity = inBuffer[w];
                var summedIntensity = localIntensity + previousSum;
                previousSum = summedIntensity;
                outBuffer[w] = summedIntensity;
            }

            for (var h = 1; h < Input.Height; h++)
            {
                var yIndex = h * width;
                var firstLocalIntensity = inBuffer[yIndex];

                var firstTopIndex = yIndex - width;
                var firstTopSumIntensity = outBuffer[firstTopIndex];

                var firstSum = firstLocalIntensity + firstTopSumIntensity;
                outBuffer[yIndex] = firstSum;

                for (var w = 1; w < width; w++)
                {
                    var index = yIndex + w;

                    var localIntensity = inBuffer[index];

                    var leftIndex = index - 1;
                    var topIndex = firstTopIndex + w;
                    var topLeftIndex = topIndex - 1;

                    var leftSumIntensity = outBuffer[leftIndex];
                    var topSumIntensity = outBuffer[topIndex];
                    var topLeftSumIntensity = outBuffer[topLeftIndex];

                    var summedIntensity = localIntensity + leftSumIntensity + topSumIntensity - topLeftSumIntensity;
                    outBuffer[index] = summedIntensity;
                }
            }
        }
    }
}