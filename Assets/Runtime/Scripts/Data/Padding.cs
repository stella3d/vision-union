namespace VisionUnion
{
    /// <summary>
    /// Specifies how many units of padding to add to each side
    /// </summary>
    public struct Padding
    {
        public int top;
        public int bottom;
        public int left;
        public int right;

        public Padding(int uniformSize)
        {
            top = uniformSize;
            bottom = uniformSize;
            left = uniformSize;
            right = uniformSize;
        }
        
        public Padding(int top, int bottom, int left, int right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }
    }
}