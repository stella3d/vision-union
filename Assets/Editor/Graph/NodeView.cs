using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using VisionUnion.Graph.Controls;
using VisionUnion.Graph.Nodes;

public class NodeView : GraphView    
{
    Color96Control m_ColorControl;
    
    public NodeView()
    {
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
        
        var convNode1 = new Convolution2d3x3Node<float>();
        AddElement(convNode1);
        var convNode2 = new Convolution2d3x3Node<float>();
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



