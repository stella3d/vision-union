using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
    static readonly List<SearchTreeEntry> k_EntryBuffer = new List<SearchTreeEntry>();

    static readonly List<Type> k_VisionNodeTypes = new List<Type>();
    static readonly List<Type> k_VisionNodeTypeBuffer = new List<Type>();
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
        k_EntryBuffer.Clear();
        k_VisionNodeTypes.Clear();
        k_VisionNodeTypeBuffer.Clear();
        m_TypeLookup.Clear();
        m_Groups.Clear();
        
        var nodeType = typeof(VisionNode);
        k_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var ass in k_Assemblies)
        {
            foreach (var type in ass.GetTypes())
            {
                if (type.IsAbstract)    // ignore types we can't instantiate
                    continue;

                // right now, we're sticking to explicit concrete implementations
                //if (type.IsGenericTypeDefinition)
                //    continue;

                NodeCategoryAttribute attribute;
                if (!TryGetCategory(type, out attribute))
                    continue;

                if (type.IsSubclassOf(nodeType))
                    k_VisionNodeTypes.Add(type);
            }
        }

        // the underlying code tries to add your first search tree entry as a group,
        // and gets a null ref if you don't because the cast. fails
        var initialGroupEntry = new SearchTreeGroupEntry(new GUIContent("Search"));
        k_Entries.Add(initialGroupEntry);
        
        foreach (var t in k_VisionNodeTypes)
        {
            m_TypeLookup.Add(t.Name, t);
            var guiContent = new GUIContent(t.Name);

            NodeCategoryAttribute attribute;
            if (!TryGetCategory(t, out attribute)) 
                continue;
            
            //Debug.Log(CategoryString(attribute));

            var depth = attribute.Length;
            var entry = new SearchTreeEntry(guiContent) {level = depth, userData = new object()};

            List<SearchTreeEntry> entries;
            SearchGroup searchGroup;
            if (m_Groups.TryGetValue(attribute[0], out searchGroup))
            {
                searchGroup.AddEntry(entry);
            }
            else
            {
                var searchContent = new GUIContent(attribute[0]);
                var firstEntry = new SearchTreeGroupEntry(searchContent) {level = 1};
                var group = new SearchGroup(firstEntry);
                group.AddEntry(entry);
                m_Groups.Add(attribute[0], group);
            }
        }

        foreach (var kvp in m_Groups)
        {
            AddGroup(kvp.Value);
        }
    }

    void AddGroup(SearchGroup group)
    {
        if (group.children.Count > 0)
        {
            foreach (var child in group.children)
            {
                AddGroup(child);
            }
        }

        foreach (var entry in group.entries)
        {
            k_Entries.Add(entry);
        }
    }

    readonly Dictionary<string, SearchGroup> m_Groups = new Dictionary<string, SearchGroup>();

    class SearchGroup
    {
        public int level;
        public List<SearchTreeEntry> entries = new List<SearchTreeEntry>();
        public List<SearchGroup> children = new List<SearchGroup>();

        public SearchGroup(SearchTreeGroupEntry firstEntry)
        {
            level = firstEntry.level;
            entries.Add(firstEntry);
        }

        public void AddEntry(SearchTreeEntry entry)
        {
            entries.Add(entry);
        }
    }

    static string CategoryString(NodeCategoryAttribute attribute)
    {
        var n = 0;
        s_String.Clear();
        s_String.Append("node category: ");
        while (n < attribute.Length - 1)
        {
            s_String.AppendFormat("{0}/", attribute[n]);
            n++;
        }

        s_String.Append(attribute[attribute.Length - 1]);
        return s_String.ToString();
    }

    static StringBuilder s_String = new StringBuilder();

    static readonly Type k_CategoryAttributeType = typeof(NodeCategoryAttribute);
    
    static bool TryGetCategory(Type t, out NodeCategoryAttribute value)
    {
        if (!Attribute.IsDefined(t, k_CategoryAttributeType))
        {
            value = null;
            return false;
        }
            
        value = (NodeCategoryAttribute)Attribute.GetCustomAttribute(t, k_CategoryAttributeType);
        return value != null;
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
