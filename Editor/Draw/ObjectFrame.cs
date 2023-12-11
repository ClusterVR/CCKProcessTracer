using UnityEditor;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public class ObjectFrame : IButton
    {
        readonly GameObject gameObject;
        readonly ProcessObject processObject;

        public Rect rect;

        public ObjectFrame(Rect rect, GameObject gameObject, ProcessObject processObject)
        {
            this.rect = rect;

            var nameButtonRect = new Rect(rect.position, new Vector2(16 + gameObject.name.Length * 10.5f, 20));

            var objectNameButton = new Button(this);
            objectNameButton.rect = nameButtonRect;
            objectNameButton.text = gameObject.name;

            this.gameObject = gameObject;
            this.processObject = processObject;

            arrowReceivePosition = new Vector2(nameButtonRect.xMin, nameButtonRect.center.y);
            arrowSendPosition = new Vector2(nameButtonRect.xMax, nameButtonRect.center.y);

            ObjectFrameDrawer.objectFrames.Add(this);
        }

        public Vector2 arrowReceivePosition
        {
            get;
        }
        public Vector2 arrowSendPosition
        {
            get;
        }

        public void OnPress()
        {
            Selection.activeGameObject = gameObject;
            processObject.folding = !processObject.folding;

            DisplayUpdater.Update();
        }
    }
}
