using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace VisionUnion.Visualization
{
    public class RGBSplitterBehaviour : MonoBehaviour
    {
        [SerializeField] Texture2D m_Input;

        Texture2D m_Red;
        Texture2D m_Green;
        Texture2D m_Blue;

        ImageDataSplitRGB<byte> m_Data;

        Vector2Int m_AspectRatio;

        RGBVisualizerBehaviour m_Visualizer;
        
        public JobHandle SplitJobHandle { get; private set; }
    
        void Awake()
        {
            m_Visualizer = GetComponent<RGBVisualizerBehaviour>();
        
            RgbSplitter.PrepareOutputTextures(m_Input, out m_Red, out m_Green, out m_Blue);
            var input = RgbSplitter.AllocateRgbChannels<Color24, byte>(m_Input, Allocator.Persistent, out m_Data);

            var channels = new []{m_Data.r, m_Data.g, m_Data.b};
            SplitJobHandle = RgbSplitter.ScheduleArraySplit(input, channels);
        
            m_Visualizer.SetTextures(m_Red, m_Green, m_Blue);
            m_Visualizer.SetImageData(m_Data);
        }

        void OnDestroy()
        {
            m_Data.Dispose();
        }
    }
}


