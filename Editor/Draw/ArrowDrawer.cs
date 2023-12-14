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
            foreach (var a in arrows)
            {
                if (!a.highlight)
                    DrawArrow(a.from, a.to, Color.gray);
            }
        }

        public static void DrawHighlightAllow()
        {
            foreach (var a in arrows)
            {
                if (a.highlight)
                    DrawArrow(a.from, a.to, Color.yellow);
            }
        }

        static void DrawArrow(Vector2 from, Vector2 to, Color color)
        {
            Vector2 p1 = (Quaternion.AngleAxis(135, Vector3.forward) * (to - from)).normalized * 10;
            Vector2 p2 = (Quaternion.AngleAxis(-135, Vector3.forward) * (to - from)).normalized * 10;

            LineDrawer.Draw(from, to, color);

            LineDrawer.Draw(to + p1, to, color);
            LineDrawer.Draw(to + p2, to, color);
            LineDrawer.Draw(to + p1, to + p2, color);
        }
    }
}
