using System;
using Unity.Jobs;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using VisionUnion.Jobs;

namespace VisionUnion.Graph.Nodes
{
    public class ConstantPadNode<T> : VisionNode
        where T: struct
    {
        readonly string m_PortLabel;
        Type m_ImageDataType;

        ImagePadJob<T> m_Job;
        
        Image<T> m_InputImage;
        Image<T> m_OutputImage;
    
        public VisionPort<Image<T>> input { get; }
        public VisionPort<Image<T>> output { get; }

        public Padding pad { get; set; }
    
        public ConstantPadNode(string titleLabel = "Constant Padding")
        {
            SetSize(new Vector2(224, 100 + style.marginTop));
            inputContainer.style.width = 84;
        
            // hardcode 1x1 padding for now.  TODO - make this have a UI
            pad = new Padding(1);
            
            title = titleLabel;
            var pixelType = typeof(T);
            m_ImageDataType = typeof(Image<T>);
            m_PortLabel = string.Format("Image<{0}>", pixelType.Name);
           
            input = VisionPort.Create<Edge, Image<T>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            input.onUpdate += OnInputUpdate;

            input.portName = m_PortLabel;
            inputContainer.Add(input);
        
            output = VisionPort.Create<Edge, Image<T>>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);

            output.portName = m_PortLabel;
            outputContainer.style.width = 84;
            outputContainer.style.flexDirection = FlexDirection.Row;
            outputContainer.style.alignItems = Align.Center;
        
            outputContainer.Add(output);

            var sizeField = new Vector2IntField();
            var sizeLabel = new Label("Pad Size");
            sizeLabel.style.minWidth = 92;
            
            var sizeContainer = new VisualContainer();
            sizeContainer.style.flexDirection = FlexDirection.Row;
            sizeContainer.style.flexWrap = Wrap.NoWrap;
            sizeField.style.flexDirection = FlexDirection.Row;
            sizeField.style.flexWrap = Wrap.NoWrap;
            sizeField.style.maxWidth = 128;
            sizeContainer.Add(sizeLabel);
            sizeContainer.Add(sizeField);

            extensionContainer.Add(sizeContainer);
        
            titleButtonContainer.style.visibility = Visibility.Hidden;
            RefreshExpandedState();
        }
        
        void OnInputUpdate(Image<T> inputImage)
        {
            if (m_OutputImage == default(Image<T>))
            {
                m_OutputImage = new Image<T>(inputImage.Width, inputImage.Height);
            }

            Debug.Log("on input update event in constant pad node");
            m_InputImage = inputImage;
            m_Job = new ImagePadJob<T>()
            {
                Input = m_InputImage,
                Output = m_OutputImage,
                Padding = pad
            };
            UpdateData();
        }

        public override void UpdateData()
        {
            m_Job.Run();
        }
    }


    public class ZeroPadNode<T> : ConstantPadNode<T>
        where T: struct
    {
        
    }
}