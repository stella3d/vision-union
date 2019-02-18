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
using VisionUnion.Graph.Controls;
using VisionUnion.Graph.Nodes;
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
        var inputNode = new Texture2dInputNode<float>(Texture2D.whiteTexture, 
            new Rect(position.x, position.y, 128, 128));
        
        var t2dNode1 = new Texture2dDisplayNode<float>
            (Texture2D.whiteTexture, new Rect(position.x, position.y, 128, 128));
        
        var t2dNode2 = new Texture2dDisplayNode<float>
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
        
        var t2dNodeMixed = new Texture2dDisplayNode<float>
            (Texture2D.whiteTexture, new Rect(position.x, position.y, 128, 128));
        
        AddElement(t2dNodeMixed);
    }

    public void OnEnable()
    {
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


