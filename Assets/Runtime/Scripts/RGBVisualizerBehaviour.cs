using Unity.Jobs;
using UnityEngine;

namespace VisionUnion.Visualization
{
    public class RGBVisualizerBehaviour : MonoBehaviour
    {
        [SerializeField] MeshRenderer m_RedRenderer;
        [SerializeField] MeshRenderer m_GreenRenderer;
        [SerializeField] MeshRenderer m_BlueRenderer;

        Texture2D m_Red;
        Texture2D m_Green;
        Texture2D m_Blue;

        ImageDataSplitRGB<byte> m_Data;

        JobHandle m_Handle;
    
        void Awake()
        {
            m_RedRenderer.material.mainTexture = m_Red;
            m_GreenRenderer.material.mainTexture = m_Green;
            m_BlueRenderer.material.mainTexture = m_Blue;
        }

        public void ApplyImageData()
        {
            m_Red.LoadImageData(m_Data.r);
            m_Green.LoadImageData(m_Data.g);
            m_Blue.LoadImageData(m_Data.b);
        }

        public void SetImageData(ImageDataSplitRGB<byte> data)
        {
            m_Data = data;
        }

        public void SetTextures(Texture2D r, Texture2D g, Texture2D b)
        {
            m_Red = r;
            m_Green = g;
            m_Blue = b;
        
            m_RedRenderer.material.mainTexture = m_Red;
            m_GreenRenderer.material.mainTexture = m_Green;
            m_BlueRenderer.material.mainTexture = m_Blue;
        }
    }
}

