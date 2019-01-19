using UnityEngine;

namespace VisionUnion
{
    public struct PoolingOptions
    {
        PoolFunction Function;
        Vector2Int KernelSize;
        Vector2Int Strides;
    }
}