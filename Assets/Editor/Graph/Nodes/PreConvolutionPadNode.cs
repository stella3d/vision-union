using System;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace VisionUnion.Graph.Nodes
{
    public class PreConvolutionPadNode<T> : VisionNode
        where T: struct
    {
        readonly string m_PortLabel;
        Type m_ImageDataType;
    
        public Port output { get; }
    
        public PreConvolutionPadNode(string titleLabel = "Pre-Convolution Padding")
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
        
            inputContainer.Add(input1);
        
            output = VisionPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                m_ImageDataType);

            output.portName = m_PortLabel;
            outputContainer.style.width = 84;
            outputContainer.style.flexDirection = FlexDirection.Row;
            outputContainer.style.alignItems = Align.Center;
        
            outputContainer.Add(output);

            var modeField = new EnumField(ConvolutionPadMode.Same);
            var sizeField = new Vector2IntField();
            var sizeLabel = new Label("Convolution Size");
            sizeLabel.style.minWidth = 92;

            
            var modeContainer = new VisualContainer();
            modeContainer.style.flexDirection = FlexDirection.Row;
            modeContainer.style.flexWrap = Wrap.NoWrap;
            
            /*
            var sizeContainer = new VisualContainer();
            sizeContainer.style.flexDirection = FlexDirection.Row;
            sizeContainer.style.flexWrap = Wrap.NoWrap;
            sizeField.style.flexDirection = FlexDirection.Row;
            sizeField.style.flexWrap = Wrap.NoWrap;
            sizeField.style.maxWidth = 128;
            sizeContainer.Add(sizeLabel);
            sizeContainer.Add(sizeField);
            */

            modeField.style.minWidth = 64;
            var padLabel = new Label("Pad Mode");
            padLabel.style.minWidth = 96;
            modeContainer.Add(padLabel);
            modeContainer.Add(modeField);
            
            //extensionContainer.Add(sizeContainer);
            extensionContainer.Add(modeContainer);
        
            titleButtonContainer.style.visibility = Visibility.Hidden;
            RefreshExpandedState();
        }
    }
}