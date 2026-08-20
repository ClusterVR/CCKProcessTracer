using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class ArrowDrawer
    {
        public static void Draw()
        {
            Color color = UnityEditor.EditorGUIUtility.isProSkin 
                ? new Color(0.5f, 0.62f, 0.7f, 0.8f) 
                : new Color(0.12f, 0.22f, 0.28f, 0.6f);
            float thickness = Mathf.Max(1f, 2.5f * View.scale);

            foreach (var connect in RefObjectFactory.connects)
            {
                if (connect == null || connect.from == null || connect.to == null) continue;

                Vector2 from = connect.from.arrowSendPosition;
                Vector2 to = connect.to.arrowReceivePosition;

                DrawArrow(from, to, connect.highlight ? new Color(1f, 0.78f, 0.2f, 1.0f) : color, connect.highlight ? Mathf.Max(1.5f, 4f * View.scale) : thickness);
            }
        }

        static void DrawArrow(Vector2 from, Vector2 to, Color color, float thickness)
        {
            Vector2 p1 = (Quaternion.AngleAxis(135, Vector3.forward) * (to - from)).normalized * 10;
            Vector2 p2 = (Quaternion.AngleAxis(-135, Vector3.forward) * (to - from)).normalized * 10;

            LineDrawer.Draw(from, to, color, thickness);
            LineDrawer.Draw(to + p1, to, color, thickness);
            LineDrawer.Draw(to + p2, to, color, thickness);
            LineDrawer.Draw(to + p1, to + p2, color, thickness);
        }
    }
}
