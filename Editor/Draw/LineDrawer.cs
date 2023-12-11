using UnityEditor;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public class LineDrawer : MonoBehaviour
    {
        public static void Draw(Vector2 from, Vector2 to, Color color)
        {
            Handles.color = color;
            Handles.DrawLine(View.ProcessViewPosition(from), View.ProcessViewPosition(to));
        }
    }
}
