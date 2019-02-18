namespace VisionUnion
{
    /// <summary>
    /// Methods for padding an image or tensor - mirrors TensorFlow options 
    /// </summary>
    public enum PadMode : short
    {
        None,
        Constant,        // Only supported mode right now
        Reflect,
        Symmetric
    }
}