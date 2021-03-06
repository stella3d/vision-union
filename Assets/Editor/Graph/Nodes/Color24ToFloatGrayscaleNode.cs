using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using VisionUnion.Jobs;

namespace VisionUnion.Graph.Nodes
{
    public class Color24ToFloatGrayscaleNode : VisionNode
    {
        Image<Color24> m_InputImage;
        Image<float> m_OutputImage;

        public VisionPort<Image<Color24>> input { get; }
        public Port output { get; }

        GreyscaleByLuminanceFloatJob24 m_Job;

        public Color24ToFloatGrayscaleNode()
        {
            SetSize(new Vector2(288, 74 + style.marginTop));
            inputContainer.style.width = 84;
        
            title = "RGB to Grayscale";
            
            input = VisionPort.Create<Edge, Image<Color24>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            input.onUpdate += OnInputUpdate;
            
            input.portName = "RGB Image<Color24>";
            inputContainer.Add(input);
            
            output = VisionPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                typeof(Image<float>));

            output.portName = "Gray Image<Single>";
            outputContainer.Add(output);
        
            titleButtonContainer.style.visibility = Visibility.Hidden;
            RefreshExpandedState();
        }

        void OnInputUpdate(Image<Color24> rgbImage, JobHandle dependency)
        {
            m_Dependency = dependency;
            if (m_OutputImage == default(Image<float>))
            {
                m_OutputImage = new Image<float>(rgbImage.Width, rgbImage.Height);
            }
            else if (m_OutputImage.Width != rgbImage.Width || m_OutputImage.Height != rgbImage.Height)
            {
                Debug.Log("output image dimensions different from input");
                m_OutputImage.Dispose();
                m_OutputImage = new Image<float>(rgbImage.Width, rgbImage.Height);
            }

            Debug.Log("on input update event in grayscale conversion node");
            m_InputImage = rgbImage;
            m_Job = new GreyscaleByLuminanceFloatJob24()
            {
                Weights = LuminanceWeights.FloatNormalized,
                InputTexture = rgbImage.Buffer,
                Grayscale = m_OutputImage.Buffer
            };
            UpdateData();
        }
        
        // TODO - separate job execution from output passing
        public override void UpdateData()
        {
            // TODO - not this, proper scheduling
            m_JobHandle = m_Job.Schedule(m_Job.InputTexture.Length, 4096, m_Dependency);
            m_JobHandle.Complete();
            
            foreach(var edge in output.connections)
            {
                var edgeInput = edge.input as VisionPort<Image<float>>;
                edgeInput?.UpdateData(m_OutputImage);
            }
        }
    }
    
    [NodeCategory("Processing", "RGB <--> Grayscale", "Color24 -->")]
    public class Color24ToFloat3Node : VisionNode
    {
        Image<Color24> m_InputImage;
        Image<float3> m_OutputImage;

        public VisionPort<Image<Color24>> input { get; }
        public VisionPort<Image<float3>> output { get; }

        Color24ToFloat3Job m_Job;

        public Color24ToFloat3Node()
        {
            SetSize(new Vector2(288, 74 + style.marginTop));
            inputContainer.style.width = 84;
        
            title = "Color24 To Float3";
            
            input = VisionPort.Create<Edge, Image<Color24>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            input.onUpdate += OnInputUpdate;
            
            input.portName = "Image<Color24>";
            inputContainer.Add(input);
            
            output = VisionPort.Create<Edge, Image<float3>>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);

            output.portName = "Image<float3>";
            outputContainer.Add(output);
        
            titleButtonContainer.style.visibility = Visibility.Hidden;
            RefreshExpandedState();
        }

        void OnInputUpdate(Image<Color24> rgbImage, JobHandle dependency)
        {
            m_Dependency = dependency;
            if (m_OutputImage == default(Image<float3>))
            {
                m_OutputImage = new Image<float3>(rgbImage.Width, rgbImage.Height);
            }
            else if (m_OutputImage.Width != rgbImage.Width || m_OutputImage.Height != rgbImage.Height)
            {
                Debug.Log("output image dimensions different from input");
                m_OutputImage.Dispose();
                m_OutputImage = new Image<float3>(rgbImage.Width, rgbImage.Height);
            }

            Debug.Log("on input update event in grayscale conversion node");
            m_InputImage = rgbImage;
            m_Job = new Color24ToFloat3Job()
            {
                Weights = LuminanceWeights.FloatNormalized,
                Input = rgbImage.Buffer,
                Output = m_OutputImage.Buffer
            };
            UpdateData();
        }
        
        // TODO - separate job execution from output passing
        public override void UpdateData()
        {
            // TODO - not this, proper scheduling
            m_JobHandle = m_Job.Schedule(m_Job.Input.Length, 4096, m_Dependency);
            m_JobHandle.Complete();
            
            foreach(var edge in output.connections)
            {
                var edgeInput = edge.input as VisionPort<Image<float3>>;
                edgeInput?.UpdateData(m_OutputImage);
            }
        }
    }
    
    //[NodeCategory("Processing", "Format Conversion")]
    public class FormatConversionNode<TFrom, TTo, TJob> : VisionNode
        where TFrom: struct
        where TTo: struct
        where TJob : struct, IFormatConversionJob<TFrom, TTo>
    {
        TFrom m_Weights;
        Image<TFrom> m_InputImage;
        Image<TTo> m_OutputImage;
        
        public VisionPort<Image<TFrom>> input { get; }
        public VisionPort<Image<TTo>> output { get; }

        TJob m_Job;

        public FormatConversionNode()
        {
            SetSize(new Vector2(288, 74 + style.marginTop));
            inputContainer.style.width = 84;

            var fromType = typeof(TFrom);
            var toType = typeof(TTo);
            title = string.Format("Convert {0} to {1}", fromType.Name, toType.Name);
            
            input = VisionPort.Create<Edge, Image<TFrom>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            input.onUpdate += OnInputUpdate;
            
            input.portName = string.Format("Image<{0}>", fromType.Name);
            inputContainer.Add(input);
            
            output = VisionPort.Create<Edge, Image<TTo>>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);

            output.portName = string.Format("Image<{0}>", toType.Name);
            outputContainer.Add(output);
        
            titleButtonContainer.style.visibility = Visibility.Hidden;
            RefreshExpandedState();
        }

        void OnInputUpdate(Image<TFrom> rgbImage, JobHandle dependency)
        {
            m_Dependency = dependency;
            if (m_OutputImage == default(Image<TTo>))
            {
                m_OutputImage = new Image<TTo>(rgbImage.Width, rgbImage.Height);
            }
            else if (m_OutputImage.Width != rgbImage.Width || m_OutputImage.Height != rgbImage.Height)
            {
                Debug.Log("output image dimensions different from input");
                m_OutputImage.Dispose();
                m_OutputImage = new Image<TTo>(rgbImage.Width, rgbImage.Height);
            }

            Debug.Log("on input update event in grayscale conversion node");
            m_InputImage = rgbImage;
            
            m_Job = new TJob();
            m_Job.SetData(rgbImage.Buffer, m_OutputImage.Buffer);
            UpdateData();
        }
        
        // TODO - separate job execution from output passing
        public override void UpdateData()
        {
            // TODO - not this, proper scheduling
            m_JobHandle = m_Job.Schedule(m_InputImage.Buffer.Length, 4096, m_Dependency);
            m_JobHandle.Complete();
            
            foreach(var edge in output.connections)
            {
                var edgeInput = edge.input as VisionPort<Image<TTo>>;
                edgeInput?.UpdateData(m_OutputImage);
            }
        }
    }
}