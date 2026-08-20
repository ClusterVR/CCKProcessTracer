using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class NodeDrawer
    {
        public static void Draw()
        {
            foreach (var node in RefObjectFactory.refObjects.SelectMany(o => o.nodes))
            {
                if (node == null || node.refObject == null) continue;

                Vector2 upLeft = View.ProcessViewPosition(new Vector2(node.rect.xMin, node.rect.yMin));
                Vector2 downRight = View.ProcessViewPosition(new Vector2(node.rect.xMax, node.rect.yMax));

                Rect scaledRect = new Rect(upLeft.x, upLeft.y, downRight.x - upLeft.x, downRight.y - upLeft.y);

                float radius = 8f * View.scale;
                if (radius < 1f) radius = 1f;

                ObjectFrameDrawer.DrawRoundedRectFill(scaledRect, radius, Color.white);

                Color borderColor = new Color(0.12f, 0.22f, 0.28f, 1.0f);
                ObjectFrameDrawer.DrawRoundedRectBorder(scaledRect, radius, borderColor, Mathf.Max(1f, 1.5f * View.scale));

                float scaledHeaderHeight = Node.nameHeight * View.scale;
                float borderY = scaledRect.yMin + scaledHeaderHeight;
                if (borderY < scaledRect.yMax)
                {
                    Handles.color = borderColor;
                    Handles.DrawLine(new Vector2(scaledRect.xMin, borderY), new Vector2(scaledRect.xMax, borderY));
                }

                float keyHeight = Node.keyHeight;
                for (int i = 0; i < node.useKeys.Count; i++)
                {
                    float keyBorderY = borderY + (keyHeight * (i + 1)) * View.scale;
                    if (keyBorderY < scaledRect.yMax)
                    {
                        Handles.color = borderColor;
                        Handles.DrawLine(new Vector2(scaledRect.xMin, keyBorderY), new Vector2(scaledRect.xMax, keyBorderY));
                    }
                }
            }
        }
    }
}
