namespace VisionUnion
{
    public struct ImageDataRGB<T> where T: struct
    {
        public ImageData<T> r;
        public ImageData<T> g;
        public ImageData<T> b;

        public ImageDataRGB(ImageData<T> r, ImageData<T> g, ImageData<T> b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}