using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using VisionUnion.Graph;

public sealed class NodeViewWindow : EditorWindow
{
    [MenuItem("VisionUnion/NodeViewWindow")]
    public static void ShowExample()
    {
        NodeViewWindow wnd = GetWindow<NodeViewWindow>();
        wnd.titleContent = new GUIContent("NodeViewWindow");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = this.GetRootVisualContainer();
        root.SetSize(new Vector2(1280, 720));


        var nodeEditorView = new NodeGraphEditorView(this);
        nodeEditorView.AddStyleSheetPath("Assets/Editor/Graph/NodeView_style.uss");
        root.Add(nodeEditorView);
    }
}


