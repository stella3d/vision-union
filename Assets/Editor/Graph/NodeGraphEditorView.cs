using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using VisionUnion.Graph.Nodes;

namespace VisionUnion.Graph
{
    public class NodeGraphEditorView : VisualElement
    {
        NodeView m_View;
        NodeSearchWindowProvider m_SearchProvider;

        SelectionDragger m_SelectionDragger = new SelectionDragger();
        ContentDragger m_ContentDragger = new ContentDragger();
        
        public NodeView view => m_View;
        
        public NodeGraphEditorView(EditorWindow window)
        {
            m_SearchProvider = ScriptableObject.CreateInstance<NodeSearchWindowProvider>();

            m_View = new NodeView()
            {
                name = "GraphView",
                persistenceKey = "NodeView"
            };

                
            m_View.SetupZoom(0.05f, ContentZoomer.DefaultMaxScale);

            m_SelectionDragger.panSpeed *= 1f;
            m_SelectionDragger.clampToParentEdges = true;
            m_ContentDragger.panSpeed *= 1f;
            m_SelectionDragger.clampToParentEdges = true;
            m_View.AddManipulator(m_SelectionDragger);
            m_View.AddManipulator(m_ContentDragger);
            m_View.AddManipulator(new RectangleSelector());
            m_View.AddManipulator(new ClickSelector());

            m_View.graphViewChanged += GraphViewChanged;


            m_View.nodeCreationRequest = (c) =>
            {
                //Debug.Log("node create request: " + c);
                SearchWindow.Open(new SearchWindowContext(c.screenMousePosition, 332, 128), m_SearchProvider);
            };

            //LoadElements();

            Add(m_View);
        }
        
        
        GraphViewChange GraphViewChanged(GraphViewChange graphViewChange)
        {
            //Debug.Log("graph move elements: " + graphViewChange.movedElements);
            //Debug.Log("graph edges to create: " + graphViewChange.edgesToCreate);
            //Debug.Log("graph move delta: " + graphViewChange.moveDelta);
            return graphViewChange;
        }
    }
}

public static class ReflectionUtil
{
    public static void FindTypes()
    {
        
    }
}

public class NodeSearchWindowProvider : ScriptableObject, ISearchWindowProvider
{
    public static NodeSearchWindowProvider Instance { get; private set; }
    
    static readonly List<SearchTreeEntry> k_Entries = new List<SearchTreeEntry>();

    static readonly List<Type> k_VisionNodeTypes = new List<Type>();
    static Assembly[] k_Assemblies;

    void OnEnable()
    {
        SetupTree();
    }

    void SetupTree()
    {
        Instance = this;
        k_VisionNodeTypes.Clear();
        var nodeType = typeof(VisionNode);
        k_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var ass in k_Assemblies)
        {
            foreach (var type in ass.GetTypes())
            {
                if (type.IsSubclassOf(nodeType))
                    k_VisionNodeTypes.Add(type);
            }
        }

        foreach (var t in k_VisionNodeTypes)
        {
            var guiContent = new GUIContent(t.Name);
            var entry = new SearchTreeEntry(guiContent);
            entry.userData = new object();
            k_Entries.Add(entry);
        }
    }
    
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        //Debug.Log("search window context: " + context);
        return k_Entries;
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        //Debug.Log("on select entry");
        return true;
    }
}
