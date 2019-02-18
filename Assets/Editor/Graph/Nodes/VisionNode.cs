using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

namespace VisionUnion.Graph.Nodes
{
    public class VisionNode : Node
    {
        public VisionNode()
        {
            AddToClassList("Node");
            SetSize(new Vector2(256, 128));
        }
    }
}