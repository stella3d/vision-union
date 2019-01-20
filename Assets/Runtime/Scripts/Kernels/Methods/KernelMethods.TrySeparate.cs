using Unity.Collections;

namespace VisionUnion
{
    public static partial class KernelMethods
    {
        public static bool TrySeparate(this Kernel<byte> kernel)
        {
            /*
             *   [~,I] = max(sum(abs(h))); % Pick the column with largest values
             *   h1 = h(:,I);
             *   [~,I] = max(sum(abs(h),2)); % Pick the row with largest values
             *   h2 = h(I,:)/h1(I);
             *   isequal(h1*h2,h)
             * 
             */

            return false;
        }
    }
}