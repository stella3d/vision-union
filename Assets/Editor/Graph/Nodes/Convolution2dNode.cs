using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace VisionUnion.Graph.Nodes
{
    public class Convolution2dNode<T> : VisionNode
        where T: struct
    {
        Convolution2D<T> m_Convolution;
        public Port Output { get; }
        
        public Convolution2dNode()
        {
            title = "2D Convolution";
            SetSize(new Vector2(272, 132));
    
            var inputImage = CustomPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                typeof(ImageData<T>));
    
            inputImage.portName = string.Format("ImageData<{0}>", typeof(T).Name);
            inputImage.style.fontSize = 9;
    
            inputContainer.Add(inputImage);
            inputContainer.style.width = this.style.width / 2;
            
            Output = CustomPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                typeof(ImageData<T>));
            
            Output.portName = string.Format("ImageData<{0}>", typeof(T).Name);
            Output.style.fontSize = 9;
            
            outputContainer.Add(Output);
            outputContainer.style.width = this.style.width / 2;
            
            SetupFloatFields();
            contentContainer.Add(extensionContainer);
        }
    
        void SetupFloatFields(int width = 3, int height = 3)
        {
            style.alignContent = new StyleValue<Align>(Align.Center);
            style.alignItems = new StyleValue<Align>(Align.Center);
            style.flexDirection = new StyleValue<FlexDirection>(FlexDirection.Row);
            
            const int pad = 4;
            var fieldWidth = (this.style.width / width) - pad - 2;
            var fieldHeight = (this.style.height / height / 2) - pad;
    
            for (var i = 0; i < width; i++)
            {
                var x = (fieldWidth + pad) * i;
                var container = new VisualContainer();
                container.style.width = fieldWidth;
                container.style.flexDirection = FlexDirection.Row;
                for (var j = 0; j < height; j++)
                {
                    var input = new FloatField(4);
                    input.style.width = fieldWidth - 18;
                    input.style.positionLeft = x;
                    input.style.positionTop = fieldHeight * j;
                    input.style.positionType = PositionType.Absolute;
                    container.Add(input);
                }
                
                Add(container);
            }
        }
    }
}