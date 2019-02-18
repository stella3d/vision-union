using System;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace VisionUnion.Graph.Nodes
{
    public abstract class ImageMixNode<T> : VisionNode
        where T: struct
    {
        Texture2D m_Texture;
        Rect m_Rect;

        readonly string m_PortLabel;
        Type m_ImageDataType;
    
        public Port output { get; }
    
        protected ImageMixNode(string titleLabel = "Image Mix")
        {
            SetSize(new Vector2(224, 100 + style.marginTop));
            inputContainer.style.width = 84;
        
            title = titleLabel;
            var pixelType = typeof(T);
            m_ImageDataType = typeof(Image<T>);
            m_PortLabel = string.Format("Image<{0}>", pixelType.Name);
           
            var input1 = VisionPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                m_ImageDataType);

            input1.portName = m_PortLabel;
        
            var input2 = VisionPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                m_ImageDataType);

            input2.portName = m_PortLabel;
        
            inputContainer.Add(input1);
            inputContainer.Add(input2);
        
            output = VisionPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                m_ImageDataType);

            output.portName = m_PortLabel;
            outputContainer.style.width = 84;
            outputContainer.style.flexDirection = FlexDirection.Row;
            outputContainer.style.alignItems = Align.Center;
        
            outputContainer.Add(output);
        
            titleButtonContainer.style.visibility = Visibility.Hidden;
            RefreshExpandedState();
        }

        public abstract void Mix();
    }

    public class FloatSquareMeanImageMixNode : ImageMixNode<float>
    {
        public FloatSquareMeanImageMixNode() : base("Mix using Square Mean") { }
    
        public override void Mix()
        {
            // TODO - implement processing here
        }

        public override void UpdateData()
        {
            throw new NotImplementedException();
        }
    }
}