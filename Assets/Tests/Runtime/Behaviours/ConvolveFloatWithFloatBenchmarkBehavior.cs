using Unity.Collections;
using VisionUnion;
using VisionUnion.Jobs;

public class ConvolveFloatWithFloatBenchmarkBehavior : JobBenchmarkBehavior<FloatWithFloatConvolveJob>
{
    protected override FloatWithFloatConvolveJob CreateJob()
    {
        return new FloatWithFloatConvolveJob()
        {
            Convolution = new Convolution2D<float>(Kernels.Float.GaussianBlurApproximate3x3),
            Input = Util.RandomFloatImage(256, 256, 0f),
            Output = new Image<float>(256, 256, Allocator.TempJob),
        };
    }
}
