using System;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace VisionUnion.Graph
{
    public class VisionPort : Port
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
        
        public VisionPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) 
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
        
        public new static VisionPort Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) 
            where TEdge : Edge, new()
        {
            var connectorListener = new OverrideEdgeConnectorListener();
            var ele = new VisionPort(orientation, direction, capacity, type)
            {
                m_EdgeConnector = new EdgeConnector<TEdge>(connectorListener)
            };
            ele.AddManipulator(ele.m_EdgeConnector);
            return ele;
        }
        
        public static VisionPort<TData> Create<TEdge, TData>(Orientation orientation, Direction direction, Capacity capacity) 
            where TEdge : Edge, new()
        {
            var connectorListener = new OverrideEdgeConnectorListener();
            var ele = new VisionPort<TData>(orientation, direction, capacity, typeof(TData))
            {
                m_EdgeConnector = new EdgeConnector<TEdge>(connectorListener)
            };
            ele.AddManipulator(ele.m_EdgeConnector);
            return ele;
        }
    }
    
    public class VisionPort<T> : VisionPort
    {
        T Value;

        public Action<T> onUpdate;
        
        public VisionPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) 
            : base(portOrientation, portDirection, portCapacity, type)
        {
        }
        
        public virtual void UpdateData(T value)
        {
            Value = value;
            onUpdate?.Invoke(value);
        }
    }
}