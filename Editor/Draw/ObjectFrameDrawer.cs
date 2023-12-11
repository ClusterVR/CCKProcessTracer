using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public class ObjectFrameDrawer : MonoBehaviour
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
                Handles.color = Color.gray;

                Vector3 upLeft = View.ProcessViewPosition(new Vector2(f.rect.xMin, f.rect.yMin));
                Vector3 upRight = View.ProcessViewPosition(new Vector2(f.rect.xMax, f.rect.yMin));
                Vector3 downRight = View.ProcessViewPosition(new Vector2(f.rect.xMax, f.rect.yMax));
                Vector3 downLeft = View.ProcessViewPosition(new Vector2(f.rect.xMin, f.rect.yMax));

                Handles.DrawLine(upLeft, upRight);
                Handles.DrawLine(upRight, downRight);
                Handles.DrawLine(downRight, downLeft);
                Handles.DrawLine(downLeft, upLeft);
            }
        }
    }
}
