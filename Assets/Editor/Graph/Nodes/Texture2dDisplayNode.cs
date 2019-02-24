using System;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using VisionUnion.Jobs;

namespace VisionUnion.Graph.Nodes
{
    public static class CommonResources
    {
        public static Material GrayscaleFloatMaterial { get; private set; }
        
        public static ComputeShader RFloatToRGBAFloatCompute { get; private set; }
        public static ComputeShader RGBFloatToRGBAFloatCompute { get; private set; }

        static string k_GrayFloatMaterialSearch = "t:Material FloatVisualizer";
        static string k_ComputeSearch = "t: ComputeShader Grayscale";
        const string RGB2RGBAFloatComputeSearch = "t: ComputeShader RGBFloat3ToRGBAFloat4";

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
            
            var computeGuids2 = AssetDatabase.FindAssets(RGB2RGBAFloatComputeSearch);
            if (computeGuids2.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(computeGuids2[0]);
                RGBFloatToRGBAFloatCompute = AssetDatabase.LoadAssetAtPath<ComputeShader>(path);
            }
        }
    }

    public class Texture2dDisplayNode<T> : VisionNode
        where T: struct
    {
        protected Image<T> m_InputImage;

        protected Texture2D m_Texture;
        protected RenderTexture m_RenderTexture;
        protected Rect m_Rect;

        protected IMGUIContainer m_ImGui;

        protected ComputeShader m_DisplayConversionCompute;

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

        protected virtual void OnGUI()
        {
            Graphics.DrawTexture(m_Rect, m_RenderTexture);
        }
        
        public virtual void OnInputUpdate(Image<T> image, JobHandle dependency)
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
    
    public class Texture2dDisplayNodeFloat : Texture2dDisplayNode<float>
    {
        public Texture2dDisplayNodeFloat(Texture2D texture, Rect rect) : base(texture, rect)
        {
            m_DisplayConversionCompute = CommonResources.RFloatToRGBAFloatCompute;
        }
    }
    
    public class Texture2dDisplayNodeFloat3 : Texture2dDisplayNode<float3>
    {
        //protected Image<float4> m_Float4Image;
        
        Float3ToFloat4Job m_Float4ConvertJob;
        
        public Texture2dDisplayNodeFloat3(Texture2D texture, Rect rect) : base(texture, rect)
        {
            m_DisplayConversionCompute = CommonResources.RGBFloatToRGBAFloatCompute;

            m_Float4ConvertJob = new Float3ToFloat4Job
            {
                Output = texture.GetRawTextureData<float4>(),
                Alpha = 1f
            };
        }
        
        protected override void OnGUI()
        {
            Graphics.DrawTexture(m_Rect, m_Texture);
        }
        
        public override void OnInputUpdate(Image<float3> image, JobHandle dependency)
        {
            /*
            if (image.Width != m_Float4Image.Width || image.Height != m_Float4Image.Height)
            {
                m_Float4Image.Buffer.DisposeIfCreated();
                m_Float4Image = new Image<float4>(image.Width, image.Height);
            }
            */

            Debug.Log("on input update event in float3 image display node");

            m_InputImage = image;
            m_Float4ConvertJob.Input = image.Buffer;
            
            
            m_InputImage = image;
            m_JobHandle = m_Float4ConvertJob.Schedule(m_Float4ConvertJob.Output.Length, 4096, dependency);
            m_JobHandle.Complete();
            m_Texture.LoadImageData(m_Float4ConvertJob.Output);
            try
            {
                //Graphics.Blit(m_Texture, m_RenderTexture);
                //Graphics.CopyTexture(m_Texture, m_RenderTexture);
            }
            catch (Exception e)
            {
                // BLACK HOLE
            }

            /*
            var kernelNumber = m_DisplayConversionCompute.FindKernel("CSMain");
            m_DisplayConversionCompute.SetTexture(kernelNumber, "Input", m_Texture);
            m_DisplayConversionCompute.SetTexture(kernelNumber, "Result", m_RenderTexture);
            m_DisplayConversionCompute.Dispatch(kernelNumber, m_Texture.width, m_Texture.height, 1);
            */
        }
    }
}