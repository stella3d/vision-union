using UnityEngine;
using VisionUnion.Visualization;

namespace VisionUnion.Examples
{
    [RequireComponent(typeof(RGBSplitterBehaviour))]
    [RequireComponent(typeof(RGBVisualizerBehaviour))]
    public class RGBCopyExampleBehaviour : MonoBehaviour
    {
        RGBSplitterBehaviour m_Splitter;
        RGBVisualizerBehaviour m_Visualizer;
    
        void Start()
        {
            m_Splitter = GetComponent<RGBSplitterBehaviour>();
            m_Visualizer = GetComponent<RGBVisualizerBehaviour>();
        }

        void Update()
        {
            switch (Time.frameCount)
            {
                case 2:
                    m_Splitter.SplitJobHandle.Complete();
                    break;
                case 3:
                    m_Visualizer.ApplyImageData();
                    break;
            }
        }
    }
}

