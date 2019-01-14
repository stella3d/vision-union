using UnityEngine;

namespace BurstVision
{
    public struct PoolingOptions
    {
        PoolFunction Function;
        Vector2Int KernelSize;
        Vector2Int Strides;
    }
}