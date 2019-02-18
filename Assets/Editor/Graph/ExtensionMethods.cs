using System.Text;
using UnityEditor.Experimental.UIElements.GraphView;

namespace VisionUnion.Graph
{
    public static class ExtensionMethods
    {
        static StringBuilder s_String = new StringBuilder();
    
        public static string PrettyPrint(this Edge edge)
        {
            s_String.Clear();
            s_String.AppendFormat("input node: {0}\n", edge.input?.node);
            s_String.AppendFormat("output node: {0}\n", edge.output?.node);
            return s_String.ToString();
        }
    }
}