using System.Collections.Generic;
using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class ButtonDrawer
    {
        public static List<Button> buttons = new List<Button>();

        public static void Clear()
        {
            buttons = new List<Button>();
        }

        public static void Draw()
        {
            foreach (var button in buttons)
            {
                if (button == null || button.Owner == null) continue;

                var scaledRect = new Rect();
                var pos = View.ProcessViewPosition(new Vector2(button.rect.x, button.rect.y));
                scaledRect.x = pos.x;
                scaledRect.y = pos.y;
                scaledRect.width = button.rect.width * View.scale;
                scaledRect.height = button.rect.height * View.scale;

                var style = new GUIStyle();
                style.alignment = TextAnchor.MiddleLeft;
                style.padding = new RectOffset((int)(10 * View.scale), (int)(10 * View.scale), 0, 0);
                style.fontSize = (int)(12 * View.scale);
                if (style.fontSize < 8) style.fontSize = 8;

                if (button.Owner is ObjectFrame)
                {
                    style.fontStyle = FontStyle.Bold;
                    style.normal.textColor = new Color(0.12f, 0.22f, 0.28f, 1.0f);
                    style.hover.textColor = new Color(0.2f, 0.35f, 0.45f, 1.0f);

                    if (scaledRect.Contains(Event.current.mousePosition))
                    {
                        Color hoverBgColor = new Color(0.12f, 0.22f, 0.28f, 0.08f);
                        UnityEditor.EditorGUI.DrawRect(scaledRect, hoverBgColor);
                    }
                }
                else if (button.Owner is Node)
                {
                    style.fontStyle = FontStyle.Bold;
                    style.normal.textColor = new Color(0.12f, 0.22f, 0.28f, 1.0f);
                    style.hover.textColor = new Color(0.2f, 0.35f, 0.45f, 1.0f);

                    if (scaledRect.Contains(Event.current.mousePosition))
                    {
                        Color hoverBgColor = new Color(0.12f, 0.22f, 0.28f, 0.05f);
                        UnityEditor.EditorGUI.DrawRect(scaledRect, hoverBgColor);
                    }
                }
                else if (button.Owner is Key)
                {
                    style.fontStyle = FontStyle.Normal;
                    style.normal.textColor = new Color(0.15f, 0.25f, 0.3f, 1.0f);
                    style.hover.textColor = new Color(0.3f, 0.5f, 0.6f, 1.0f);

                    if (scaledRect.Contains(Event.current.mousePosition))
                    {
                        Color hoverBgColor = new Color(0.12f, 0.22f, 0.28f, 0.05f);
                        UnityEditor.EditorGUI.DrawRect(scaledRect, hoverBgColor);
                    }
                }

                if (GUI.Button(scaledRect, button.text, style))
                {
                    button.OnPress();
                }
            }
        }
    }
}
