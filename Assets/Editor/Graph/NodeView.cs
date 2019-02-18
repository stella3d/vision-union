using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using VisionUnion;
using Edge = UnityEditor.Experimental.UIElements.GraphView.Edge;


public class NodeView : GraphView    
{
    Color96Control m_ColorControl;
    
    public NodeView()
    {
        //Debug.Log("NodeView Constructor");
        AddStyleSheetPath("NodeView_style");
        SetSize(new Vector2(960, 540));
        
        style.alignItems = StyleValue<Align>.Create(Align.FlexStart);

        SetupSobelExample(transform.position);
    }

    static readonly List<Port> k_Ports = new List<Port>();
    static readonly List<Port> k_CompatiblePorts = new List<Port>();

    // this override is necessary - the default implementation does not work
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        Debug.Log("custom GetCompatiblePorts");
        var portType = startPort.portType;
        var direction = startPort.direction;

        ports.ToList(k_Ports);
        foreach (var port in k_Ports)
        {
            if (port.portType != portType || port.direction == direction)
                continue;

            k_CompatiblePorts.Add(port);
        }

        return k_CompatiblePorts;
    }

    void SetupSobelExample(Vector3 position)
    {
        var inputNode = new Texture2DInputNode<float>(Texture2D.whiteTexture, 
            new Rect(position.x, position.y, 128, 128));
        
        var t2dNode1 = new Texture2DPreviewNode<float>
            (Texture2D.whiteTexture, new Rect(position.x, position.y, 128, 128));
        
        var t2dNode2 = new Texture2DPreviewNode<float>
            (Texture2D.whiteTexture, new Rect(position.x, position.y, 128, 128));
        
        AddElement(inputNode);
        AddElement(t2dNode1);
        AddElement(t2dNode2);
        
        var convNode1 = new Convolution2dNode<float>();
        AddElement(convNode1);
        var convNode2 = new Convolution2dNode<float>();
        AddElement(convNode2);

        var mixNode = new FloatSquareMeanImageMixNode();
        AddElement(mixNode);
        
        var t2dNodeMixed = new Texture2DPreviewNode<float>
            (Texture2D.whiteTexture, new Rect(position.x, position.y, 128, 128));
        
        AddElement(t2dNodeMixed);
    }

    public void OnEnable()
    {
    }
}

public class CustomNode : Node
{
    public CustomNode()
    {
        AddToClassList("Node");
        SetSize(new Vector2(256, 128));
    }
}

public static class Extensions
{
    static StringBuilder s_String = new StringBuilder();
    
    public static string PrettyPrint(this Edge edge)
    {
        s_String.Clear();
        s_String.AppendFormat("input node: {0}\n", edge.input?.node);
        s_String.AppendFormat("output node: {0}\n", edge.output?.node);
        return s_String.ToString();
    }
}

public class CustomPort : Port
{
    class OverrideEdgeConnectorListener : IEdgeConnectorListener
    {
        private GraphViewChange m_GraphViewChange;
        private List<Edge> m_EdgesToCreate;
        private List<GraphElement> m_EdgesToDelete;

        public OverrideEdgeConnectorListener()
        {
            this.m_EdgesToCreate = new List<Edge>();
            this.m_EdgesToDelete = new List<GraphElement>();
            this.m_GraphViewChange.edgesToCreate = this.m_EdgesToCreate;
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            Debug.Log("Dropped outside port ?");
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            Debug.Log("onDrop for custom edge listener");
            m_EdgesToCreate.Clear();
            m_EdgesToCreate.Add(edge);
            m_EdgesToDelete.Clear();
            if (edge.input.capacity == Capacity.Single)
            {
                foreach (Edge connection in edge.input.connections)
                {
                    if (connection != edge)
                        this.m_EdgesToDelete.Add(connection);
                }
            }
            if (edge.output.capacity == Capacity.Single)
            {
                foreach (Edge connection in edge.output.connections)
                {
                    if (connection != edge)
                        m_EdgesToDelete.Add(connection);
                }
            }
            if (m_EdgesToDelete.Count > 0)
                graphView.DeleteElements(m_EdgesToDelete);
            List<Edge> edgesToCreate = m_EdgesToCreate;
            if (graphView.graphViewChanged != null)
                edgesToCreate = graphView.graphViewChanged(m_GraphViewChange).edgesToCreate;
            foreach (Edge edge1 in edgesToCreate)
            {
                graphView.AddElement(edge1);
                edge.input.Connect(edge1);
                edge.output.Connect(edge1);
            }
        }
    }
    
    public CustomPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) 
        : base(portOrientation, portDirection, portCapacity, type)
    {
    }

    public override void OnStopEdgeDragging()
    {
        base.OnStopEdgeDragging();
        Debug.Log("on stop edge drag");
        var edge = edgeConnector.edgeDragHelper.edgeCandidate;
        if (edge == null)
            return;
        
        Debug.Log(edge.PrettyPrint());
    }
    
    public new static CustomPort Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) 
        where TEdge : Edge, new()
    {
        var connectorListener = new OverrideEdgeConnectorListener();
        var ele = new CustomPort(orientation, direction, capacity, type)
        {
            m_EdgeConnector = new EdgeConnector<TEdge>(connectorListener)
        };
        ele.AddManipulator(ele.m_EdgeConnector);
        return ele;
    }
}

public class Convolution2dNode<T> : CustomNode
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

public class Texture2DPreviewNode<T> : CustomNode
    where T: struct
{
    Texture2D m_Texture;
    Rect m_Rect;

    IMGUIContainer m_ImGui;

    public Port input { get; }
    
    public Texture2DPreviewNode(Texture2D texture, Rect rect)
    {
        var labelSize = style.fontSize * 4 + 4;
        var size = new Vector2(rect.width, rect.height + labelSize);
        var textureSize = new Vector2(rect.width, rect.height);
        
        SetSize(new Vector2(132, rect.height + 78 + style.marginTop));
        m_Rect = rect;
        m_Texture = texture;
        m_ImGui = new IMGUIContainer(OnGUI);

        m_ImGui.SetSize(textureSize);
        m_ImGui.style.positionType = new StyleValue<PositionType>(PositionType.Relative);
        
        title = "Image Preview";
        input = CustomPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
            typeof(ImageData<T>));

        input.portName = string.Format("ImageData<{0}>", typeof(T).Name);
        
        inputContainer.Add(input);
        
        extensionContainer.SetSize(textureSize);
        extensionContainer.Add(m_ImGui);
        extensionContainer.style.positionType = PositionType.Relative;
        contentContainer.Add(extensionContainer);
        RefreshExpandedState();
    }

    void OnGUI()
    {
        EditorGUI.DrawPreviewTexture(m_Rect, m_Texture);
    }
}


public class Texture2DInputNode<T> : CustomNode
    where T: struct
{
    Texture2D m_Texture;
    Rect m_Rect;

    public Port output { get; }
    
    public Texture2DInputNode(Texture2D texture, Rect rect)
    {
        //var labelSize = style.fontSize * 4 + 4;
        //var size = new Vector2(rect.width, rect.height + labelSize);
        //var textureSize = new Vector2(rect.width, rect.height);
        
        SetSize(new Vector2(272, 78 + style.marginTop));
        inputContainer.style.width = 84;
        
        m_Rect = rect;
        m_Texture = texture;

        title = "Texture Input";
        output = CustomPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
            typeof(ImageData<T>));

        output.portName = string.Format("ImageData<{0}>", typeof(T).Name);
        
        outputContainer.Add(output);
        
        var objField = TypedObjectField<Texture2D>();
        
        inputContainer.Add(objField);
        titleButtonContainer.style.visibility = Visibility.Hidden;
        RefreshExpandedState();
    }

    static ObjectField TypedObjectField<TObject>()
    {
        var objField = new ObjectField();
        objField.objectType = typeof(TObject);
        return objField;
    }

    void OnGUI()
    {
        EditorGUI.DrawPreviewTexture(m_Rect, m_Texture);
    }
}

public abstract class ImageMixNode<T> : CustomNode
    where T: struct
{
    Texture2D m_Texture;
    Rect m_Rect;

    readonly string m_PortLabel;
    Type m_ImageDataType;
    
    public Port output { get; }
    
    protected ImageMixNode(string titleLabel = "Image Mix")
    {
        SetSize(new Vector2(272, 100 + style.marginTop));
        inputContainer.style.width = 84;
        
        title = titleLabel;
        var pixelType = typeof(T);
        m_ImageDataType = typeof(ImageData<T>);
        m_PortLabel = string.Format("ImageData<{0}>", pixelType.Name);
           
        var input1 = CustomPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
            m_ImageDataType);

        input1.portName = m_PortLabel;
        
        var input2 = CustomPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
            m_ImageDataType);

        input2.portName = m_PortLabel;
        
        inputContainer.Add(input1);
        inputContainer.Add(input2);
        
        output = CustomPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
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
}

public class Color96Control : BaseField<Color96>
{
    public Color96Control()
    {
        AddToClassList("ScalarValueControl");
        var field = new ColorField {showAlpha = false};
        Add(field);
    }
}

public class FloatControl : BaseField<float>
{
    FloatField m_ValueField;
    
    public FloatControl(string labelText)
    {
        var label = new Label(labelText);

        m_ValueField = new FloatField {value = 0.50f, maxLength = 4};
        m_ValueField.style.marginLeft += 16;

        label.Add(m_ValueField);
        Add(label);
    }
}



