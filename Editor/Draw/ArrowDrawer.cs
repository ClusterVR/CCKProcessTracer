using System.Collections.Generic;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class ArrowDrawer : MonoBehaviour
    {
        public static List<Arrow> arrows = new List<Arrow>();

        public static void Clear()
        {
            arrows.Clear();
        }

        public static void DrawNormalArrow()
        {
            Color color = UnityEditor.EditorGUIUtility.isProSkin 
                ? new Color(0.5f, 0.62f, 0.7f, 0.8f) 
                : new Color(0.12f, 0.22f, 0.28f, 0.6f);
            float thickness = Mathf.Max(1f, 2.5f * View.scale);

            foreach (var a in arrows)
            {
                if (!a.highlight && KeyFilter.IsArrowVisible(a))
                    DrawArrow(a.from, a.to, color, thickness);
            }
        }

        public static void DrawHighlightAllow()
        {
            Color color = new Color(1f, 0.78f, 0.2f, 1.0f);
            float thickness = Mathf.Max(1.5f, 4f * View.scale);

            foreach (var a in arrows)
            {
                if (a.highlight && KeyFilter.IsArrowVisible(a))
                    DrawArrow(a.from, a.to, color, thickness);
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
