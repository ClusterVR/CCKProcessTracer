using UnityEditor;
using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class TracerWindow : EditorWindow
    {
        void Update()
        {
            Repaint();
        }

        void OnEnable()
        {
            RefObjectFactory.Create();
            DisplayUpdater.Update();
        }

        void OnGUI()
        {
            wantsMouseMove = true;
            DragController.Control();

            ObjectFrameDrawer.Draw();
            NodeDrawer.Draw();
            ArrowDrawer.Draw();
            ButtonDrawer.Draw();

            if (GUI.Button(new Rect(position.size.x - 150, 0, 150, 30), "UpdateNodes"))
            {
                RefObjectFactory.Create();
                DisplayUpdater.Update();
            }
            if (GUI.Button(new Rect(position.size.x - 150, 30, 150, 30), "ResetView"))
            {
                View.Reset();
                foreach (var obj in RefObjectFactory.refObjects)
                {
                    if (obj != null) obj.dragOffset = Vector2.zero;
                }
                DisplayUpdater.Update();
            }

            View.Control();
        }

        [MenuItem("CCKProcessTracer/OpenWorldItemRefTracerWindow")]
        static void Open()
        {
            var window = GetWindow<TracerWindow>();
            window.titleContent = new GUIContent("WorldItemRefTracer");
            window.minSize = new Vector2(350, 100);
        }
    }
}
