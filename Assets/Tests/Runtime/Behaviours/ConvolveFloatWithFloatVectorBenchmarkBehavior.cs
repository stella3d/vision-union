using Unity.Collections;
using Unity.Mathematics;
using VisionUnion;
using VisionUnion.Jobs;

public class ConvolveFloatWithFloatVectorBenchmarkBehavior : JobBenchmarkBehavior<FloatWithFloat3VectorConvolveJob>
{
    protected override FloatWithFloat3VectorConvolveJob CreateJob()
    {
        return new FloatWithFloat3VectorConvolveJob()
        {
            Convolution = new Convolution2D<float>(Kernels.Float.GaussianBlurApproximate3x3),
            Input = Util.RandomFloat3Image(256, 256, 0f),
            Output = new Image<float3>(256, 256, Allocator.TempJob),
        };
    }
}
