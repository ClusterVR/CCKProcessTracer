using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class ObjectFrameDrawer : MonoBehaviour
    {
        public static List<ObjectFrame> objectFrames = new List<ObjectFrame>();

        public static void Clear()
        {
            objectFrames.Clear();
        }

        public static void Draw()
        {
            foreach (var f in objectFrames)
            {
                if (!KeyFilter.IsFrameVisible(f.processObject))
                    continue;

                Vector2 upLeft = View.ProcessViewPosition(new Vector2(f.rect.xMin, f.rect.yMin));
                Vector2 downRight = View.ProcessViewPosition(new Vector2(f.rect.xMax, f.rect.yMax));

                Rect scaledRect = new Rect(upLeft.x, upLeft.y, downRight.x - upLeft.x, downRight.y - upLeft.y);

                float radius = 12f * View.scale;
                if (radius < 1f) radius = 1f;

                // 背景塗りつぶし
                DrawRoundedRectFill(scaledRect, radius, new Color(0.93f, 0.94f, 0.95f, 1.0f));

                // 枠線
                DrawRoundedRectBorder(scaledRect, radius, new Color(0.12f, 0.22f, 0.28f, 1.0f), Mathf.Max(1f, 3f * View.scale));
            }
        }

        public static void DrawRoundedRectFill(Rect rect, float radius, Color color)
        {
            var points = GetRoundedRectPoints(rect, radius);
            Handles.color = color;
            Handles.DrawAAConvexPolygon(points.ToArray());
        }

        public static void DrawRoundedRectBorder(Rect rect, float radius, Color color, float thickness)
        {
            var points = GetRoundedRectPoints(rect, radius);
            // 閉じた線にするため、最初の点を最後にも追加する
            points.Add(points[0]);
            Handles.color = color;
            Handles.DrawAAPolyLine(thickness, points.ToArray());
        }

        private static List<Vector3> GetRoundedRectPoints(Rect rect, float radius)
        {
            int segments = 8;
            List<Vector3> points = new List<Vector3>();

            // 左上
            AddCorner(points, new Vector2(rect.xMin + radius, rect.yMin + radius), radius, 180f, 270f, segments);
            // 右上
            AddCorner(points, new Vector2(rect.xMax - radius, rect.yMin + radius), radius, 270f, 360f, segments);
            // 右下
            AddCorner(points, new Vector2(rect.xMax - radius, rect.yMax - radius), radius, 0f, 90f, segments);
            // 左下
            AddCorner(points, new Vector2(rect.xMin + radius, rect.yMax - radius), radius, 90f, 180f, segments);

            return points;
        }

        private static void AddCorner(List<Vector3> points, Vector2 center, float radius, float startAngle, float endAngle, int segments)
        {
            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Lerp(startAngle, endAngle, (float)i / segments) * Mathf.Deg2Rad;
                points.Add(new Vector3(center.x + Mathf.Cos(angle) * radius, center.y + Mathf.Sin(angle) * radius, 0));
            }
        }
    }
}
