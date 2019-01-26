using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using VisionUnion;

public class RGBVisualizerBehaviour : MonoBehaviour
{
    [SerializeField] Texture2D m_Input;
    
    [SerializeField] MeshRenderer m_RedRenderer;
    [SerializeField] MeshRenderer m_GreenRenderer;
    [SerializeField] MeshRenderer m_BlueRenderer;

    Texture2D m_Red;
    Texture2D m_Green;
    Texture2D m_Blue;

    ImageDataSplitRGB<byte> m_Data;

    Vector2Int m_AspectRatio;

    JobHandle m_Handle;
    
    void Awake()
    {
        RgbSplitter.PrepareOutputTextures(m_Input, out m_Red, out m_Green, out m_Blue);
        var input = RgbSplitter.AllocateRgbChannels<Color24, byte>(m_Input, Allocator.Persistent, out m_Data);

        var channels = new []{m_Data.r, m_Data.g, m_Data.b};
        m_Handle = RgbSplitter.ScheduleArraySplit(input, channels);
        
        m_RedRenderer.material.mainTexture = m_Red;
        m_GreenRenderer.material.mainTexture = m_Green;
        m_BlueRenderer.material.mainTexture = m_Blue;
    }

    void OnDestroy()
    {
        m_Data.Dispose();
    }

    public void ApplyImageData()
    {
        m_Red.LoadImageData(m_Data.r);
        m_Green.LoadImageData(m_Data.g);
        m_Blue.LoadImageData(m_Data.b);
    }

    void Update()
    {
        switch(Time.frameCount)
        {
            case 5:
                m_Handle.Complete();
                ApplyImageData();
                break;
        }
    }
}
