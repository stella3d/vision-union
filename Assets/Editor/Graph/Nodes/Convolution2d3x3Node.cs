using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace VisionUnion.Graph.Nodes
{
    public class Convolution2d3x3Node<T> : VisionNode
        where T: struct
    {
        Convolution2D<T> m_Convolution;
        
        protected Image<T> m_InputImage;
        protected Image<T> m_OutputImage;
    
        public VisionPort<Image<T>> input { get; protected set; }
        public VisionPort<Image<T>> output { get; protected set; }
        
        public Convolution2d3x3Node()
        {
            title = "3x3 2D Convolution";
            SetSize(new Vector2(224, 132));
    
            input = VisionPort.Create<Edge, Image<T>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
    
            input.portName = string.Format("Image<{0}>", typeof(T).Name);
            input.style.fontSize = 9;
    
            inputContainer.Add(input);
            inputContainer.style.width = this.style.width / 2;
            
            output = VisionPort.Create<Edge, Image<T>>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
            
            output.portName = string.Format("Image<{0}>", typeof(T).Name);
            output.style.fontSize = 9;
            
            outputContainer.Add(output);
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
        
        void OnInputUpdate(Image<T> inputImage)
        {
            if (m_OutputImage == default(Image<T>))
            {
                m_OutputImage = new Image<T>(inputImage.Width, inputImage.Height);
            }
        }

        public override void UpdateData()
        {
            throw new System.NotImplementedException();
        }
    }
}