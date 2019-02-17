using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using VisionUnion.Graph;

public sealed class NodeViewWindow : EditorWindow
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

        var nodeEditorView = new NodeGraphEditorView(this);
        nodeEditorView.AddStyleSheetPath("Assets/Editor/Graph/NodeView_style.uss");
        root.Add(nodeEditorView);
    }
}


