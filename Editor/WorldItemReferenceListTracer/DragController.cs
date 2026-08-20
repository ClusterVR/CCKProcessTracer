using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public static class DragController
    {
        public static RefObject draggingObject { get; private set; }
        private static Vector2 dragStartOffset;

        public static void Control()
        {
            Event e = Event.current;
            if (e == null) return;

            Vector2 mousePos = e.mousePosition;
            Vector2 canvasMousePos = View.ScreenToCanvasPosition(mousePos);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                RefObject foundObject = null;
                for (int i = ObjectFrameDrawer.objectFrames.Count - 1; i >= 0; i--)
                {
                    var frame = ObjectFrameDrawer.objectFrames[i];
                    if (frame == null || frame.refObject == null) continue;

                    if (frame.nameButtonRect.Contains(canvasMousePos))
                    {
                        foundObject = frame.refObject;
                        break;
                    }
                }

                if (foundObject != null)
                {
                    draggingObject = foundObject;
                    dragStartOffset = canvasMousePos - new Vector2(foundObject.objectFrame.nameButtonRect.x, foundObject.objectFrame.nameButtonRect.y);
                    e.Use();
                }
            }
            else if (e.type == EventType.MouseDrag && e.button == 0)
            {
                if (draggingObject != null)
                {
                    Vector2 basePos = new Vector2(draggingObject.objectFrame.nameButtonRect.x, draggingObject.objectFrame.nameButtonRect.y) - draggingObject.dragOffset;
                    Vector2 targetPos = canvasMousePos - dragStartOffset;
                    draggingObject.dragOffset = targetPos - basePos;
                    
                    DisplayUpdater.Update();
                    e.Use();
                }
            }
            else if (e.type == EventType.MouseUp && e.button == 0)
            {
                if (draggingObject != null)
                {
                    draggingObject = null;
                    e.Use();
                }
            }
        }
    }
}
