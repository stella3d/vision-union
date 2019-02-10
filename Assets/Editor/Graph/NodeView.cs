using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEditor.Graphs;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using VisionUnion;


public class NodeView : GraphView
{
    GraphView m_GraphView;

    Color96Control m_ColorControl;
    
    public NodeView()
    {
        Debug.Log("NodeView Constructor");
        AddStyleSheetPath("NodeView_style");
        SetSize(new Vector2(960, 540));
        
        style.alignItems = StyleValue<Align>.Create(Align.FlexStart);

        var colorNode = new Color96Node();
        AddElement(colorNode);
        
        var color = new Color96(0.1f, 0.5f, 0.25f);
    }

    public void OnEnable()
    {
    }
}

public abstract class Node : GraphElement
{
    public Node()
    {
        AddToClassList("Node");
        SetSize(new Vector2(256, 128));
    }
}

public class Color96Node : Node
{
    public Color96Node()
    {
        SetSize(new Vector2(140, 80));
        var control = new Color96Control();
        control.style.flexDirection = new StyleValue<FlexDirection>(FlexDirection.Row);
        Add(control);

        var port = new Color96Port(Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
        port.style.maxWidth = 72f;
        port.source = new PortSource<Color96>();
        port.style.flexDirection = new StyleValue<FlexDirection>(FlexDirection.Row);
        Add(port);
    }
}

public class Color96Port : Port
{
    public Color96Port(Orientation portOrientation, Direction portDirection, Capacity portCapacity) 
        : base(portOrientation, portDirection, portCapacity, typeof(Color96))
    {
    }
}

public class Color96Control : BaseField<Color96>
{
    public Color96Control()
    {
        AddToClassList("ScalarValueControl");
        Add(new FloatControl("r"));
        Add(new FloatControl("g"));
        Add(new FloatControl("b"));
    }
}

public class FloatValueField : TextValueField<float>
{
    public FloatValueField(int maxLength) : base(maxLength)
    {
        AddToClassList("ScalarValueField");
    }

    public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, float startValue)
    {
        //
    }

    protected override string ValueToString(float value)
    {
        return value.ToString("F3");
    }

    protected override float StringToValue(string str)
    {
        float f;
        float.TryParse(str, out f);
        return f;
    }

    protected override string allowedCharacters { get; } = ".0123456789";
}

public class FloatControl : BaseField<float>
{
    FloatField m_ValueField;
    
    public FloatControl(string labelText)
    {
        var label = new Label(labelText);
        //label.style.flexDirection = new StyleValue<FlexDirection>(FlexDirection.Row);
        //label.style.alignItems = new StyleValue<Align>(Align.FlexStart);
        

        m_ValueField = new FloatField();
        m_ValueField.value = 0.50f;
        m_ValueField.maxLength = 4;
        m_ValueField.style.marginLeft += 16;

        label.Add(m_ValueField);
        Add(label);
        //Add(group);
    }
}



