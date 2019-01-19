using UnityEngine;

namespace VisionUnion
{
    public struct PoolingOptions
    {
        public PoolFunction Function;
        public Vector2Int KernelSize;
        public Vector2Int Strides;
    }
}