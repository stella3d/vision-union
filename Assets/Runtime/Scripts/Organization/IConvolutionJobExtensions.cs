using Unity.Jobs;

namespace VisionUnion.Organization
{
    public interface IConvolutionJob<TKernel> : IJob
        where TKernel: struct
    {
        void SetConvolution(Convolution<TKernel> convolution);
        
        void SetData(Convolution<TKernel> convolution, ImageData<TKernel> input, ImageData<TKernel> output);
    }
}