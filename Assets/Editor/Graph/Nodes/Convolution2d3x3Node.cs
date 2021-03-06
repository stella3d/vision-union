using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using VisionUnion.Jobs;

namespace VisionUnion.Graph.Nodes
{
    public class Convolution2d3x3Node<TKernel, TImage> : VisionNode
        where TKernel: struct
        where TImage: struct
    {
        protected Convolution2D<TKernel> m_Convolution;
        
        protected Image<TImage> m_InputImage;
        protected Image<TImage> m_OutputImage;

        protected readonly TKernel[,] MatrixInputValues = new TKernel[3, 3];

        public VisionPort<Image<TImage>> input { get; protected set; }
        public VisionPort<Image<TImage>> output { get; protected set; }
        
        public Convolution2d3x3Node()
        {
            title = "3x3 2D Convolution";
            SetSize(new Vector2(224, 132));
            
            m_Convolution = new Convolution2D<TKernel>(new Kernel2D<TKernel>(3, 3));
    
            input = VisionPort.Create<Edge, Image<TImage>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            input.onUpdate += OnInputUpdate;
    
            input.portName = string.Format("Image<{0}>", typeof(TImage).Name);
            input.style.fontSize = 9;
    
            inputContainer.Add(input);
            inputContainer.style.width = this.style.width / 2;
            
            output = VisionPort.Create<Edge, Image<TImage>>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
            
            output.portName = string.Format("Image<{0}>", typeof(TImage).Name);
            output.style.fontSize = 9;
            
            outputContainer.Add(output);
            outputContainer.style.width = this.style.width / 2;
            
            SetupInputFields();
            contentContainer.Add(extensionContainer);
        }

        ~Convolution2d3x3Node()
        {
            m_Convolution.Dispose();
        }

        protected virtual void SetupInputFields(int width = 3, int height = 3) {}
        
        protected virtual void SetupJob() { }

        void OnInputUpdate(Image<TImage> inputImage, JobHandle dependency)
        {
            //Debug.Log("conv node input update");
            if (m_OutputImage.Width != inputImage.Width || m_OutputImage.Height != inputImage.Height)
            {
                m_OutputImage.Buffer.DisposeIfCreated();
                m_OutputImage = new Image<TImage>(inputImage.Width, inputImage.Height);
            }

            m_InputImage = inputImage;
            SetupJob();
            UpdateData();
        }

        public override void UpdateData() { }
    }
    
    public class FloatConvolution2d3x3Node : Convolution2d3x3Node<float, float>
    {
        protected readonly FloatField[,] MatrixInputs = new FloatField[3,3];

        protected FloatWithFloatConvolveJob m_Job;
        
        protected override void SetupInputFields(int width = 3, int height = 3)
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
                    var input = new FloatField(6);
                    input.style.width = fieldWidth - 16;
                    input.style.positionLeft = x;
                    input.style.positionTop = fieldHeight * j;
                    input.style.positionType = PositionType.Absolute;
                    input.OnValueChanged(OnMatrixInputUpdate);
                    MatrixInputs[i, j] = input;
                    container.Add(input);
                }
                
                Add(container);
            }
        }

        protected override void SetupJob()
        {
            m_Job = new FloatWithFloatConvolveJob
            {
                Input = m_InputImage,
                Output = m_OutputImage,
                Convolution = m_Convolution
            };
        }
        
        void OnMatrixInputUpdate(ChangeEvent<float> changeEvent)
        {
            for (var y = 0; y < 3; y++)
            {
                for (var x = 0; x < 3; x++)
                {
                    MatrixInputValues[x, y] = MatrixInputs[x, y].value;
                }
            }
            
            m_Convolution.Kernel2D.SetWeights(MatrixInputValues);
            
            if(!m_InputImage.Buffer.IsCreated)
            {
                //Debug.Log("no image, not running convolve job");
                return;
            }
            
            // this is where we need to start actually scheduling things probably...
            UpdateData();
        }
        
        public override void UpdateData()
        {
            m_Job.Run();

            foreach(var edge in output.connections)
            {
                var edgeInput = edge.input as VisionPort<Image<float>>;
                edgeInput?.UpdateData(m_OutputImage);
            }
        }
    }
    
    public class Float3Convolution2d3x3Node : Convolution2d3x3Node<float, float3>
    {
        protected readonly FloatField[,] MatrixInputs = new FloatField[3,3];

        protected FloatWithFloat3ConvolveJob m_Job;
        
        protected override void SetupInputFields(int width = 3, int height = 3)
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
                    var input = new FloatField(6);
                    input.style.width = fieldWidth - 16;
                    input.style.positionLeft = x;
                    input.style.positionTop = fieldHeight * j;
                    input.style.positionType = PositionType.Absolute;
                    input.OnValueChanged(OnMatrixInputUpdate);
                    MatrixInputs[i, j] = input;
                    container.Add(input);
                }
                
                Add(container);
            }
        }

        protected override void SetupJob()
        {
            m_Job = new FloatWithFloat3ConvolveJob
            {
                Input = m_InputImage,
                Output = m_OutputImage,
                Convolution = m_Convolution
            };
        }
        
        void OnMatrixInputUpdate(ChangeEvent<float> changeEvent)
        {
            for (var y = 0; y < 3; y++)
            {
                for (var x = 0; x < 3; x++)
                {
                    MatrixInputValues[x, y] = MatrixInputs[x, y].value;
                }
            }
            
            m_Convolution.Kernel2D.SetWeights(MatrixInputValues);
            
            if(!m_InputImage.Buffer.IsCreated)
            {
                Debug.Log("no image, not running convolve job");
                return;
            }
            
            // this is where we need to start actually scheduling things probably...
            UpdateData();
        }
        
        public override void UpdateData()
        {
            m_Job.Run();

            foreach(var edge in output.connections)
            {
                var edgeInput = edge.input as VisionPort<Image<float3>>;
                edgeInput?.UpdateData(m_OutputImage);
            }
        }
    }
}