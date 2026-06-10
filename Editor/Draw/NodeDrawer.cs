using UnityEditor;
using UnityEngine;

namespace CCKProcessTracer.Editor
{
    public sealed class NodeDrawer
    {
        public static void Draw()
        {
            foreach (var node in NodeFactory.nodes)
            {
                if (node == null || node.processObject == null)
                    continue;

                if (!KeyFilter.IsFrameVisible(node.processObject))
                    continue;

                Vector2 upLeft = View.ProcessViewPosition(new Vector2(node.rect.xMin, node.rect.yMin));
                Vector2 downRight = View.ProcessViewPosition(new Vector2(node.rect.xMax, node.rect.yMax));

                Rect scaledRect = new Rect(upLeft.x, upLeft.y, downRight.x - upLeft.x, downRight.y - upLeft.y);

                float radius = 8f * View.scale;
                if (radius < 1f) radius = 1f;

                // カード背景（白）
                ObjectFrameDrawer.DrawRoundedRectFill(scaledRect, radius, Color.white);

                // カード枠線（ダークブルー）
                Color borderColor = new Color(0.12f, 0.22f, 0.28f, 1.0f);
                ObjectFrameDrawer.DrawRoundedRectBorder(scaledRect, radius, borderColor, Mathf.Max(1f, 1.5f * View.scale));

                // ヘッダー下部の境界線
                float nameHeight = 50f; // Node.nameHeight の実値
                float scaledHeaderHeight = nameHeight * View.scale;
                float borderY = scaledRect.yMin + scaledHeaderHeight;
                if (borderY < scaledRect.yMax)
                {
                    Handles.color = borderColor;
                    Handles.DrawLine(new Vector2(scaledRect.xMin, borderY), new Vector2(scaledRect.xMax, borderY));
                }

                // 各キー行の下部境界線（TriggerNodeなどキーを持つ場合）
                if (node is TriggerNode triggerNode)
                {
                    float keyHeight = 25f; // Node.keyHeight の実値
                    for (int i = 0; i < triggerNode.useKeys.Count; i++)
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
}
