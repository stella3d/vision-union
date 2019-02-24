using Unity.Jobs;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

namespace VisionUnion.Graph.Nodes
{
    public abstract class VisionNode : Node
    {
        /// <summary>
        /// The handle for the job this node depends on.
        /// If the node has multiple input dependencies, they need to be combined here.
        /// </summary>
        protected JobHandle m_Dependency;
        
        /// <summary>
        /// The handle for the job this node schedules to do its work.  Passed to all outputs.
        /// </summary>
        protected JobHandle m_JobHandle;
        
        public VisionNode()
        {
            AddToClassList("Node");
            SetSize(new Vector2(256, 128));
        }

        public abstract void UpdateData();
    }
}