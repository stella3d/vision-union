using System;
using Unity.Jobs;
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
        
        public static ComputeShader RFloatToRGBAFloatCompute { get; private set; }

        static string k_GrayFloatMaterialSearch = "t:Material FloatVisualizer";
        static string k_ComputeSearch = "t: ComputeShader Grayscale";

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
            
            var computeGuids = AssetDatabase.FindAssets(k_ComputeSearch);
            if (computeGuids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(computeGuids[0]);
                RFloatToRGBAFloatCompute = AssetDatabase.LoadAssetAtPath<ComputeShader>(path);
            }
        }
    }

    public class Texture2dDisplayNode<T> : VisionNode
        where T: struct
    {
        Image<T> m_InputImage;

        Texture2D m_Texture;
        RenderTexture m_RenderTexture;
        Rect m_Rect;

        IMGUIContainer m_ImGui;

        ComputeShader m_DisplayConversionCompute;

        public VisionPort<Image<T>> input { get; }
    
        public Texture2dDisplayNode(Texture2D texture, Rect rect)
        {
            var labelSize = style.fontSize * 4 + 4;
            var size = new Vector2(rect.width, rect.height + labelSize);
            var textureSize = new Vector2(rect.width, rect.height);
        
            SetSize(new Vector2(132, rect.height + 78 + style.marginTop));
            m_Rect = rect;
            m_Texture = texture;
            m_RenderTexture = new RenderTexture(m_Texture.width, m_Texture.height, 1, RenderTextureFormat.ARGBFloat);
            m_RenderTexture.enableRandomWrite = true;

            m_DisplayConversionCompute = CommonResources.RFloatToRGBAFloatCompute;
            
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
            Graphics.DrawTexture(m_Rect, m_RenderTexture);
        }
        
        public void OnInputUpdate(Image<T> image, JobHandle dependency)
        {
            Debug.Log("on input update event in image display node");
            m_InputImage = image;
            try
            {
                Graphics.CopyTexture(m_Texture, m_RenderTexture);
            }
            catch (Exception e)
            {
                // BLACK HOLE
            }

            m_Texture.LoadImageData(image);

            var kernelNumber = m_DisplayConversionCompute.FindKernel("CSMain");
            m_DisplayConversionCompute.SetTexture(kernelNumber, "Input", m_Texture);
            m_DisplayConversionCompute.SetTexture(kernelNumber, "Result", m_RenderTexture);
            m_DisplayConversionCompute.Dispatch(kernelNumber, m_Texture.width, m_Texture.height, 1);
        }

        public override void UpdateData()
        {
            
        }
    }
}