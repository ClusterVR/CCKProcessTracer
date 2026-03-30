using UnityEditor;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class CCKProcessTracer : EditorWindow
    {
        void Update()
        {
            Repaint();
        }

        void OnEnable()
        {
            ProcessObjectFactory.Create();
            DisplayUpdater.Update();
        }

        void OnGUI()
        {
            wantsMouseMove = true;
            DrawManager.Draw();

            if (GUI.Button(new Rect(position.size.x - 150, 0, 150, 30), "UpdateNodes"))
            {
                ProcessObjectFactory.Create();
                DisplayUpdater.Update();
            }
            if (GUI.Button(new Rect(position.size.x - 150, 30, 150, 30), "ResetView"))
            {
                View.Reset();
            }
            ProcessObjectFactory.displaySelectionOnly = GUI.Toggle(new Rect(position.size.x - 150, 60, 150, 30),
                ProcessObjectFactory.displaySelectionOnly, "Display Selection Only");

            var prevEnabled = KeyFilter.enabled;
            KeyFilter.enabled = GUI.Toggle(new Rect(position.size.x - 150, 90, 150, 30),
                KeyFilter.enabled, "Filter by Key");

            if (KeyFilter.enabled)
            {
                var prevIndex = KeyFilter.selectedKeyIndex;
                KeyFilter.selectedKeyIndex = EditorGUI.Popup(
                    new Rect(position.size.x - 150, 120, 150, 20),
                    KeyFilter.selectedKeyIndex,
                    KeyFilter.keyDisplayNames);

                if (prevIndex != KeyFilter.selectedKeyIndex)
                    KeyFilter.BuildFilterSets();
            }

            if (prevEnabled != KeyFilter.enabled)
                KeyFilter.BuildFilterSets();

            View.Control();
        }
        
        [MenuItem("CCKProcessTracer/OpenWindow")]
        static void Open()
        {
            var window = GetWindow<CCKProcessTracer>();
            window.titleContent = new GUIContent("CCKProcessTracer");
            window.minSize = new Vector2(350, 100);
        }
    }
}
