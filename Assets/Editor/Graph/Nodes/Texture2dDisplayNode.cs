using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace VisionUnion.Graph.Nodes
{
    public static class CommonResources
    {
        public static Material GrayscaleFloatMaterial { get; private set; }
        
        public static CustomRenderTexture GrayscaleRenderTexture { get; private set; }

        static string k_GrayFloatMaterialSearch = "t:Material FloatVisualizer";
        static string k_CustomRenderTextureSearch = "t: CustomRenderTexture";

        static CommonResources()
        {
            Load();
        }

        public static void Load()
        {
            var floatMatPaths = AssetDatabase.FindAssets(k_GrayFloatMaterialSearch);
            if (floatMatPaths.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(floatMatPaths[0]);
                GrayscaleFloatMaterial = AssetDatabase.LoadAssetAtPath<Material>(path);
            }
            
            var renderTexPaths = AssetDatabase.FindAssets(k_CustomRenderTextureSearch);
            if (renderTexPaths.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(renderTexPaths[0]);
                GrayscaleRenderTexture = AssetDatabase.LoadAssetAtPath<CustomRenderTexture>(path);
            }
        }
    }

    public class Texture2dDisplayNode<T> : VisionNode
        where T: struct
    {
        Image<T> m_InputImage;

        Texture2D m_Texture;
        Rect m_Rect;


        CustomRenderTexture m_RenderTexture;
        IMGUIContainer m_ImGui;

        public VisionPort<Image<T>> input { get; }
    
        public Texture2dDisplayNode(Texture2D texture, Rect rect)
        {
            var labelSize = style.fontSize * 4 + 4;
            var size = new Vector2(rect.width, rect.height + labelSize);
            var textureSize = new Vector2(rect.width, rect.height);
        
            SetSize(new Vector2(132, rect.height + 78 + style.marginTop));
            m_Rect = rect;
            m_Texture = texture;
            m_RenderTexture = CommonResources.GrayscaleRenderTexture;
            /*
            m_RenderTexture = new CustomRenderTexture(texture.width, texture.height,
                RenderTextureFormat.RFloat, RenderTextureReadWrite.Default)
            {
                updateMode = CustomRenderTextureUpdateMode.OnDemand,
                material = CommonResources.GrayscaleFloatMaterial
            };

            m_RenderTexture.material.mainTexture = m_Texture;
            */
            m_RenderTexture.Initialize();
            
            m_ImGui = new IMGUIContainer(OnGUI);

            m_ImGui.SetSize(textureSize);
            m_ImGui.style.positionType = new StyleValue<PositionType>(PositionType.Relative);
        
            title = "Image Preview";
            input = VisionPort.Create<Edge, Image<T>>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            input.onUpdate += OnInputUpdate;
            
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
            //RenderTexture.active = m_RenderTexture;
            //GL.PushMatrix();
            //GL.LoadPixelMatrix();
            if (m_RenderTexture == null)
                return;
            Graphics.DrawTexture(m_Rect, m_RenderTexture);
            //Graphics.DrawTexture(m_Rect, m_RenderTexture);
            //m_RenderTexture.Update();
            //GL.PopMatrix();
            //RenderTexture.active = null;
            //EditorGUI.DrawPreviewTexture(m_Rect, m_RenderTexture);
        }
        
        public void OnInputUpdate(Image<T> image)
        {
            Debug.Log("on input update event in image display node");
            m_InputImage = image;
            m_Texture.LoadImageData(image);
            Graphics.Blit(m_Texture, m_RenderTexture);
            //Graphics.CopyTexture(m_Texture, m_RenderTexture);
            m_RenderTexture.Update();
        }

        public override void UpdateData()
        {
            throw new System.NotImplementedException();
        }
    }
}