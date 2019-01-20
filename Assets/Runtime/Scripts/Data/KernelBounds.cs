using UnityEngine;

namespace VisionUnion
{
    public struct KernelBounds
    {
        public Vector2Int negative;
        public Vector2Int positive;

        public KernelBounds(int width, int height)
        {
            var wMod2 = width % 2;
            var hMod2 = height % 2;
            var wUnder3 = width < 3;
            var hUnder3 = height < 3;
            
            var pwOffset = wUnder3 ? width - 1 : width / 2;
            var phOffset = hUnder3 ? height - 1 : height / 2;
            var nwOffset = wUnder3 ? 0 : width / 2 - (1 - wMod2);
            var nhOffset = hUnder3 ? 0 : height / 2 - (1 - hMod2);

            negative = new Vector2Int(-nwOffset, -nhOffset);
            positive = new Vector2Int(pwOffset, phOffset);
        }
        
        public override string ToString()
        {
            return string.Format("kernel bounds: {0}, {1}", negative, positive);
        }
    }
}