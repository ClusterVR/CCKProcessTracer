using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class ObjectFrame : IButton
    {
        public readonly GameObject gameObject;
        public readonly RefObject refObject;

        public Rect rect;
        public Rect nameButtonRect;

        public ObjectFrame(Rect rect, GameObject gameObject, RefObject refObject)
        {
            this.rect = rect;
            this.gameObject = gameObject;
            this.refObject = refObject;

            var nameBtnRect = new Rect(rect.position, new Vector2(16 + gameObject.name.Length * 10.5f, 20));
            this.nameButtonRect = nameBtnRect;

            var objNameBtn = new Button(this);
            objNameBtn.rect = nameBtnRect;
            objNameBtn.text = gameObject.name;

            ObjectFrameDrawer.objectFrames.Add(this);
        }

        public void OnPress()
        {
            if (gameObject != null)
            {
                UnityEditor.Selection.activeGameObject = gameObject;
            }
        }
    }
}
