using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;

namespace VisionUnion.Graph.Controls
{
    public class Color96Control : BaseField<Color96>
    {
        public Color96Control()
        {
            AddToClassList("ScalarValueControl");
            var field = new ColorField {showAlpha = false};
            Add(field);
        }
    }
}