using System;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;
using VisionUnion.Jobs;

namespace VisionUnion.Graph.Nodes
{
    public abstract class ImageMixNode<T> : VisionNode
        where T: struct
    {
        Texture2D m_Texture;
        Rect m_Rect;

        readonly string m_PortLabel;
    
        public VisionPort<Image<T>> input1 { get; }
        public VisionPort<Image<T>> input2 { get; }
        public VisionPort<Image<T>> output { get; }
    
        protected ImageMixNode(string titleLabel = "Image Mix")
        {
            SetSize(new Vector2(224, 100 + style.marginTop));
            inputContainer.style.width = 84;
        
            title = titleLabel;
            var pixelType = typeof(T);
            m_PortLabel = string.Format("Image<{0}>", pixelType.Name);
           
            input1 = VisionPort.Create<Edge, Image<T>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            input1.portName = m_PortLabel;
        
            input2 = VisionPort.Create<Edge, Image<T>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            input2.portName = m_PortLabel;
        
            inputContainer.Add(input1);
            inputContainer.Add(input2);
        
            output = VisionPort.Create<Edge, Image<T>>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);

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
    
    [NodeCategory("Processing", "Mix")]
    public class FloatSquareMeanImageMixNode : ImageMixNode<float>
    {
        SquareCombineJobFloat m_Job;

        JobHandle m_Dependency1;
        JobHandle m_Dependency2;

        Image<float> m_OutputImage;

        public FloatSquareMeanImageMixNode() : base("Mix using Square Mean")
        {
            m_Job = new SquareCombineJobFloat();
            input1.onUpdate += OnInput1Update;
            input2.onUpdate += OnInput2Update;
        }

        ~FloatSquareMeanImageMixNode()
        {
            m_OutputImage.Buffer.DisposeIfCreated();
        }

        void OnInput1Update(Image<float> inputImage, JobHandle dependency)
        {
            InputUpdate(inputImage, dependency, out m_Job.A, out m_Dependency1, Mix);
        }
        
        void OnInput2Update(Image<float> inputImage, JobHandle dependency)
        {
            InputUpdate(inputImage, dependency, out m_Job.B, out m_Dependency2, Mix);
        }

        void InputUpdate(Image<float> inputImage, JobHandle dependency, out Image<float> jobData, 
            out JobHandle dependencyCache, Action completionCallback = null)
        {
            if (m_OutputImage.Width != inputImage.Width || m_OutputImage.Height != inputImage.Height)
            {
                m_OutputImage.Buffer.DisposeIfCreated();
                m_OutputImage = new Image<float>(inputImage.Width,inputImage.Height);
                m_Job.Output = m_OutputImage;
            }
            
            // TODO - warn / prevent incompatible images ?
            jobData = inputImage;
            dependencyCache = dependency;
            m_Dependency = JobHandle.CombineDependencies(m_Dependency1, m_Dependency2);
            completionCallback?.Invoke();
        }

        public override void Mix()
        {
            // don't mix if we don't have two inputs
            if (!m_Job.A.Buffer.IsCreated || !m_Job.B.Buffer.IsCreated)
                return;
            
            m_JobHandle = m_Job.Schedule(m_OutputImage.Buffer.Length, 4096, m_Dependency);
            // TODO - delay scheduling when asked for
            m_JobHandle.Complete();
            UpdateData();
        }

        public override void UpdateData()
        {
            foreach(var edge in output.connections)
            {
                var edgeInput = edge.input as VisionPort<Image<float>>;
                edgeInput?.UpdateData(m_OutputImage, m_JobHandle);
            }
        }
    }
    
    [NodeCategory("Processing", "Mix")]
    public class Float3SquareMeanImageMixNode : ImageMixNode<float3>
    {
        SquareCombineJobFloat3 m_Job;

        JobHandle m_Dependency1;
        JobHandle m_Dependency2;

        Image<float3> m_OutputImage;

        public Float3SquareMeanImageMixNode() : base("Mix using Square Mean")
        {
            m_Job = new SquareCombineJobFloat3();
            input1.onUpdate += OnInput1Update;
            input2.onUpdate += OnInput2Update;
        }

        ~Float3SquareMeanImageMixNode()
        {
            m_OutputImage.Buffer.DisposeIfCreated();
        }

        void OnInput1Update(Image<float3> inputImage, JobHandle dependency)
        {
            InputUpdate(inputImage, dependency, out m_Job.A, out m_Dependency1, Mix);
        }
        
        void OnInput2Update(Image<float3> inputImage, JobHandle dependency)
        {
            InputUpdate(inputImage, dependency, out m_Job.B, out m_Dependency2, Mix);
        }

        void InputUpdate(Image<float3> inputImage, JobHandle dependency, out Image<float3> jobData, 
            out JobHandle dependencyCache, Action completionCallback = null)
        {
            if (m_OutputImage.Width != inputImage.Width || m_OutputImage.Height != inputImage.Height)
            {
                m_OutputImage.Buffer.DisposeIfCreated();
                m_OutputImage = new Image<float3>(inputImage.Width,inputImage.Height);
                m_Job.Output = m_OutputImage;
            }
            
            // TODO - warn / prevent incompatible images ?
            jobData = inputImage;
            dependencyCache = dependency;
            m_Dependency = JobHandle.CombineDependencies(m_Dependency1, m_Dependency2);
            completionCallback?.Invoke();
        }

        public override void Mix()
        {
            // don't mix if we don't have two inputs
            if (!m_Job.A.Buffer.IsCreated || !m_Job.B.Buffer.IsCreated)
                return;
            
            m_JobHandle = m_Job.Schedule(m_OutputImage.Buffer.Length, 4096, m_Dependency);
            // TODO - delay scheduling when asked for
            m_JobHandle.Complete();
            UpdateData();
        }

        public override void UpdateData()
        {
            foreach(var edge in output.connections)
            {
                var edgeInput = edge.input as VisionPort<Image<float3>>;
                edgeInput?.UpdateData(m_OutputImage, m_JobHandle);
            }
        }
    }
}