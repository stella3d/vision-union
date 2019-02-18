using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace VisionUnion.Graph.Nodes
{
    public class Texture2dInputNode<T> : VisionNode
        where T: struct
    {
        Texture2D m_Texture;
        Rect m_Rect;

        public Port output { get; }
    
        public Texture2dInputNode(Texture2D texture, Rect rect)
        {
            SetSize(new Vector2(256, 78 + style.marginTop));
            inputContainer.style.width = 84;
        
            m_Rect = rect;
            m_Texture = texture;

            title = "Texture Input";
            output = CustomPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                typeof(Image<T>));

            output.portName = string.Format("Image<{0}>", typeof(T).Name);
        
            outputContainer.Add(output);
        
            var objField = TypedObjectField<Texture2D>();
        
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
    }
}