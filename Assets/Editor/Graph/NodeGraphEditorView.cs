using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace VisionUnion.Graph
{
    public class NodeGraphEditorView : VisualElement
    {
        NodeView m_View;
        ISearchWindowProvider m_SearchProvider;

        SelectionDragger m_SelectionDragger = new SelectionDragger();
        ContentDragger m_ContentDragger = new ContentDragger();
        
        public NodeView view => m_View;
        
        public NodeGraphEditorView(EditorWindow window)
        {
            m_View = new NodeView()
            {
                name = "GraphView",
                persistenceKey = "NodeView"
            };

                
            m_View.SetupZoom(0.05f, ContentZoomer.DefaultMaxScale);

            m_SelectionDragger.panSpeed *= 0.333f;
            m_ContentDragger.panSpeed *= 0.333f;
            m_View.AddManipulator(m_SelectionDragger);
            m_View.AddManipulator(m_ContentDragger);
            m_View.AddManipulator(new RectangleSelector());
            m_View.AddManipulator(new ClickSelector());

            m_View.graphViewChanged += GraphViewChanged;

            m_SearchProvider = ScriptableObject.CreateInstance<NodeSearchWindowProvider>();
            //m_SearchProvider.Initialize(editorWindow, this, _graphView);

            m_View.nodeCreationRequest = (c) =>
            {
                Debug.Log("node create request: " + c);
                //_searchWindowProvider.ConnectedPortView = null;
                //SearchWindow.Open(new SearchWindowContext(c.screenMousePosition), _searchWindowProvider);
            };

            //LoadElements();

            Add(m_View);
        }
        
        
        GraphViewChange GraphViewChanged(GraphViewChange graphViewChange)
        {
            Debug.Log("graph move elements: " + graphViewChange.movedElements);
            Debug.Log("graph edges to create: " + graphViewChange.edgesToCreate);
            //Debug.Log("graph move delta: " + graphViewChange.moveDelta);
            return graphViewChange;
        }
    }
}

public class NodeSearchWindowProvider : ScriptableObject, ISearchWindowProvider
{
    static readonly List<SearchTreeEntry> k_Entries = new List<SearchTreeEntry>();
    
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        return k_Entries;
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        Debug.Log("on select entry");
        return true;
    }
}
