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
    
    [NodeCategory("Processing", "Color Adjust", "Linear Hue")]
    public class HueAdjustNode : VisionNode
    {
        Image<float3> m_Image;

        public VisionPort<Image<float3>> input { get; set; }
        public VisionPort<Image<float3>> output { get; set; }

        LinearHueAdjustFloat3 m_Job;

        float3 m_Weights = new float3();

        Slider m_RedInput;
        Slider m_GreenInput;
        Slider m_BlueInput;

        public HueAdjustNode()
        {
            SetSize(new Vector2(288, 74 + style.marginTop));
            inputContainer.style.width = 84;
        
            title = "Hue Adjust";
            
            input = VisionPort.Create<Edge, Image<float3>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            input.onUpdate += OnInputUpdate;
            
            input.portName = "Image<float3>";
            inputContainer.Add(input);
            
            output = VisionPort.Create<Edge, Image<float3>>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);

            output.portName = "Image<float3>";
            outputContainer.Add(output);

            var redLabel = new Label("Red");
            m_RedInput = new Slider(-1f, 1f, (value) =>
            {
                m_Weights.x = value;
                UpdateData();
            });
            var redContainer = new VisualElement {redLabel, m_RedInput};
            //redContainer.style.alignContent = Align.Center;
            //redContainer.style.alignItems = Align.Center;
            redLabel.style.alignSelf = Align.Center;
            
            var greenLabel = new Label("Green");
            m_GreenInput = new Slider(-1f, 1f, (value) =>
            {
                m_Weights.y = value;
                UpdateData();
            });
            
            var greenContainer = new VisualElement {greenLabel, m_GreenInput};

            var blueLabel = new Label("Blue");
            m_BlueInput = new Slider(-1f, 1f, (value) =>
            {
                m_Weights.z = value;
                UpdateData();
            });
            
            var blueContainer = new VisualElement {blueLabel, m_BlueInput};

            extensionContainer.Add(redContainer);
            extensionContainer.Add(greenContainer);
            extensionContainer.Add(blueContainer);
        
            titleButtonContainer.style.visibility = Visibility.Hidden;
            RefreshExpandedState();
        }

        void OnInputUpdate(Image<float3> rgbImage, JobHandle dependency)
        {
            m_Dependency = dependency;

            m_Image = rgbImage;
            
            if(m_Job.Output.IsCreated)
                m_Job.Output.Dispose();
            
            m_Job = new LinearHueAdjustFloat3()
            {
                Weights = m_Weights,
                Image = rgbImage.Buffer,
                Output = new NativeArray<float3>(rgbImage.Buffer.Length, Allocator.Persistent)
            };
            UpdateData();
        }
        
        // TODO - separate job execution from output passing
        public override void UpdateData()
        {
            // TODO - not this, proper scheduling
            m_JobHandle = m_Job.Schedule(m_Job.Image.Length, 4096, m_Dependency);
            m_JobHandle.Complete();
            
            foreach(var edge in output.connections)
            {
                var edgeInput = edge.input as VisionPort<Image<float3>>;
                edgeInput?.UpdateData(m_Image);
            }
        }
    }
}