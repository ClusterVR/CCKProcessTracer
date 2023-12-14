using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class View
    {
        public static float scale = 1;
        static Vector2 scaleCenterPosition = new Vector2(0, 0);
        static Vector2 scrollPosition = new Vector2(0, 0);

        static Vector2 beforeMousePosition = Vector2.zero;

        public static void Reset()
        {
            scale = 1;
            scrollPosition = Vector2.zero;
        }

        public static void Control()
        {
            if (Event.current.type == EventType.ScrollWheel)
            {
                scaleCenterPosition = Event.current.mousePosition;
                scale += Event.current.delta.y * -.03f * scale;
                if (scale < 0.01f)
                {
                    scale = 0.01f;
                }
            }

            if (Event.current.type == EventType.MouseDown)
            {
                beforeMousePosition = Event.current.mousePosition;
            }

            if (Event.current.type == EventType.MouseDrag)
            {
                var mouseMove = Event.current.mousePosition - beforeMousePosition;

                if (mouseMove != Vector2.zero)
                {
                    scrollPosition += mouseMove / scale;

                    beforeMousePosition = Event.current.mousePosition;
                }
            }
        }

        public static Vector2 ProcessViewPosition(Vector2 position)
        {
            // 画面中心を原点として、スケールとスクロールを適用する
            var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            return (position - screenCenter + scrollPosition) * scale + screenCenter;
        }
    }
}
