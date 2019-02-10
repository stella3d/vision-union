using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;


public class NodeViewWindow : EditorWindow
{
    [MenuItem("Window/UIElements/NodeViewWindow")]
    public static void ShowExample()
    {
        NodeViewWindow wnd = GetWindow<NodeViewWindow>();
        wnd.titleContent = new GUIContent("NodeViewWindow");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = this.GetRootVisualContainer();

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C#");
        //root.Add(label);

        var nodeView = new NodeView();
        nodeView.AddStyleSheetPath("Assets/Editor/Graph/NodeView_style.uss");
        root.Add(nodeView);
        /*

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath("Assets/Editor/Graph/NodeViewWindow.uxml", typeof(VisualTreeAsset)) as VisualTreeAsset;
        VisualElement labelFromUXML = visualTree.CloneTree(null);
        root.Add(labelFromUXML);
                */

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        
    }
}