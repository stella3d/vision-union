using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace VisionUnion.Graph.Nodes
{
    internal class ImageChangeEventFloat : EventBase<ImageChangeEventFloat> {}
    
    internal class ImageChangeEvent<T> : EventBase<ImageChangeEvent<T>>
        where T: struct
    {
        public Image<T> Image;
        
        public ImageChangeEvent(ref Image<T> newValue)
        {
            Image = newValue;
        }

        public ImageChangeEvent() { }

        public void SetNewValue(ref Image<T> image)
        {
            Image= image;
        }
    }
    
    public class Texture2dInputNode<T> : VisionNode
        where T: struct
    {
        Texture2D m_Texture;
        Rect m_Rect;

        Image<T> m_Image;

        ImageChangeEvent<T> m_ImageChangeEvent;

        public Port output { get; }
    
        public Texture2dInputNode()
        {
            SetSize(new Vector2(256, 74 + style.marginTop));
            inputContainer.style.width = 84;
        
            title = "Texture Input";
            output = VisionPort.Create<Edge, Image<T>>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);

            output.portName = string.Format("Image<{0}>", typeof(T).Name);
        
            outputContainer.Add(output);
            
            var objField = TypedObjectField<Texture2D>();
            objField.OnValueChanged(evt =>
            {
                Debug.Log("on texture input value change");
                var texture = (Texture2D) evt.newValue;
                if (texture == null)
                    return;

                m_Texture = texture;
                m_Image = new Image<T>(texture);
                output.source = m_Image;
                UpdateData();
            });
            
        
            inputContainer.Add(objField);
            titleButtonContainer.style.visibility = Visibility.Hidden;
            RefreshExpandedState();
        }

        static ObjectField TypedObjectField<TObject>()
        {
            var objField = new ObjectField();
            objField.objectType = typeof(TObject);
            return objField;
        }

        public override void UpdateData()
        {
            foreach(var edge in output.connections)
            {
                var input = edge.input as VisionPort<Image<T>>;
                input?.UpdateData(m_Image);
            }
        }
    }
}