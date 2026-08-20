using UnityEditor;
using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class LineDrawer
    {
        public static void Draw(Vector2 from, Vector2 to, Color color, float thickness = 2f)
        {
            Handles.color = color;
            Vector3[] points = new Vector3[] { View.ProcessViewPosition(from), View.ProcessViewPosition(to) };
            Handles.DrawAAPolyLine(thickness, points);
        }
    }
}
