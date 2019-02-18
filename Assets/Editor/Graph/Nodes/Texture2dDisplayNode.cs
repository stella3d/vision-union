using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace VisionUnion.Graph.Nodes
{
    public class Texture2dDisplayNode<T> : VisionNode
        where T: struct
    {
        Texture2D m_Texture;
        Rect m_Rect;

        IMGUIContainer m_ImGui;

        public Port input { get; }
    
        public Texture2dDisplayNode(Texture2D texture, Rect rect)
        {
            var labelSize = style.fontSize * 4 + 4;
            var size = new Vector2(rect.width, rect.height + labelSize);
            var textureSize = new Vector2(rect.width, rect.height);
        
            SetSize(new Vector2(132, rect.height + 78 + style.marginTop));
            m_Rect = rect;
            m_Texture = texture;
            m_ImGui = new IMGUIContainer(OnGUI);

            m_ImGui.SetSize(textureSize);
            m_ImGui.style.positionType = new StyleValue<PositionType>(PositionType.Relative);
        
            title = "Image Preview";
            input = CustomPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                typeof(Image<T>));

            input.portName = string.Format("Image<{0}>", typeof(T).Name);
        
            inputContainer.Add(input);
        
            extensionContainer.SetSize(textureSize);
            extensionContainer.Add(m_ImGui);
            extensionContainer.style.positionType = PositionType.Relative;
            contentContainer.Add(extensionContainer);
            RefreshExpandedState();
        }

        void OnGUI()
        {
            EditorGUI.DrawPreviewTexture(m_Rect, m_Texture);
        }
    }
}