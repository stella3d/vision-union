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
        EditorWindow m_Window;
        NodeView m_View;
        NodeSearchWindowProvider m_SearchProvider;

        SelectionDragger m_SelectionDragger = new SelectionDragger();
        ContentDragger m_ContentDragger = new ContentDragger();
        
        public NodeView view => m_View;
        
        public NodeGraphEditorView(EditorWindow window)
        {
            m_Window = window;
            m_View = new NodeView()
            {
                name = "GraphView",
                persistenceKey = "NodeView"
            };

            m_SearchProvider = ScriptableObject.CreateInstance<NodeSearchWindowProvider>();
            m_SearchProvider.view = m_View;
                
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
                SearchWindow.Open(new SearchWindowContext(c.screenMousePosition, 256, 128), m_SearchProvider);
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

    public NodeView view { get; internal set; }
    
    void OnEnable()
    {
        SetupTree();
    }

    static readonly Dictionary<string, Type> m_TypeLookup = new Dictionary<string, Type>();
    static readonly Dictionary<string, List<SearchTreeEntry>> m_FirstLevelCategories = 
        new Dictionary<string, List<SearchTreeEntry>>();
    
    /*
     * Top level
     * Categories
     *
     * 2nd level
     * Generic type definitions
     *
     * 3rd level
     * concrete type implementations
     */
    void SetupTree()
    {
        Instance = this;
        k_Entries.Clear();
        k_VisionNodeTypes.Clear();
        m_TypeLookup.Clear();
        SetupCategories();
        
        var nodeType = typeof(VisionNode);
        k_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var ass in k_Assemblies)
        {
            foreach (var type in ass.GetTypes())
            {
                if (type.IsAbstract)    // ignore types we can't instantiate
                    continue;

                // right now, we're sticking to explicit concrete implementations
                if (type.IsGenericTypeDefinition)    
                    continue;
                
                if (type.IsSubclassOf(nodeType))
                    k_VisionNodeTypes.Add(type);
            }
        }

        // the underlying code tries to add your first search tree entry as a group,
        // and gets a null ref if you don't because the cast. fails
        var initialGroupEntry = new SearchTreeGroupEntry(new GUIContent("Vision Union"));
        k_Entries.Add(initialGroupEntry);
        foreach (var t in k_VisionNodeTypes)
        {
            m_TypeLookup.Add(t.Name, t);
            var guiContent = new GUIContent(t.Name);
            var entry = new SearchTreeEntry(guiContent) {level = 1, userData = new object()};
            k_Entries.Add(entry);
        }
    }

    void SetupCategories()
    {
        m_FirstLevelCategories.Clear();

        var displayList = new List<SearchTreeEntry> {new SearchTreeGroupEntry(new GUIContent("Display"))};
        m_FirstLevelCategories.Add("Display", displayList);
        
        var inputList = new List<SearchTreeEntry> {new SearchTreeGroupEntry(new GUIContent("Input"))};
        m_FirstLevelCategories.Add("Input", inputList);
        
        var processList = new List<SearchTreeEntry> {new SearchTreeGroupEntry(new GUIContent("Processing"))};
        m_FirstLevelCategories.Add("Processing", processList);
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        //Debug.Log("search window context: " + context);
        return k_Entries;
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        Type selectedType;
        Debug.Log("on select entry");
        if (!m_TypeLookup.TryGetValue(searchTreeEntry.name, out selectedType)) 
            return false;
        
        var instance = Activator.CreateInstance(selectedType) as VisionNode;
        if (instance == null) 
            return false;

        Add(instance, context);
        return true;
    }

    void Add(VisionNode node, SearchWindowContext context)
    {
        var size = new Vector2(context.requestedWidth, context.requestedHeight);
        var portPoint = view.viewport.WorldToLocal(context.screenMousePosition);
        var viewPosition = view.viewTransform.position;

        // TODO - what is the proper screen-to-graph transform function?
        Debug.LogFormat("screen point {0}, view position {1}, local point {2}", 
            context.screenMousePosition, viewPosition, portPoint);
        var rect = new Rect(portPoint, size);
        node.SetPosition(rect);
        view.AddElement(node);
    }
}
