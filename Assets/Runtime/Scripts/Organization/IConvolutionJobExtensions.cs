using Unity.Jobs;

namespace VisionUnion.Organization
{
    public interface IConvolutionJob<T> : IJob
        where T: struct
    {
        void SetConvolution(Convolution<T> convolution);
    }
}