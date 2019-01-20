using Unity.Collections;

namespace VisionUnion
{
    public static class Pad
    {
        public static ImageData<T> Image<T>(ImageData<T> input, Padding padding, PadMode mode, 
            T constantValue = default(T), Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            var output = default(ImageData<T>);
            switch (mode)
            {
                // constant is the only method supported so far
                case PadMode.Constant:
                    output = Constant(input, padding, constantValue, allocator);
                    break;
            }

            return output;
        }
        
        public static ImageData<T> Constant<T>(ImageData<T> input, Padding pad, T value = default(T), 
            Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            var newWidth = input.Width + pad.left + pad.right;
            var newHeight = input.Height + pad.top + pad.bottom;
           
            var newImage = new ImageData<T>(newWidth, newHeight, allocator);
            var outBuffer = newImage.Buffer;
            var inBuffer = input.Buffer;

            var inputIndex = 0;
            var outIndex = 0;
            for (var i = 0; i < pad.top; i++)                                     // top pad
            {
                for (var c = 0; c < newImage.Width; c++)
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }

            var contentRowEnd = newHeight - pad.bottom;
            for (var r = pad.top; r < contentRowEnd; r++)
            {
                var contentColumnEnd = newWidth - pad.right;
                for (var i = 0; i < pad.left; i++)                               // left pad
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
                for (var i = pad.left; i < contentColumnEnd; i++)                // original content
                {
                    outBuffer[outIndex] = inBuffer[inputIndex];
                    outIndex++;
                    inputIndex++;
                }
                for (var i = contentColumnEnd; i < newWidth; i++)                // right pad
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }
            
            for (var r = contentRowEnd; r < newHeight; r++)                      // bottom pad
            {
                for (var c = 0; c < newImage.Width; c++)
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }

            return newImage;
        }
    }
}