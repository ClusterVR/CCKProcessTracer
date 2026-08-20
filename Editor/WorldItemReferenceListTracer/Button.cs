using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class Button
    {
        readonly IButton parent;
        public IButton Owner => parent;
        public Rect rect;
        public string text;
        public Button(IButton parent)
        {
            ButtonDrawer.buttons.Add(this);
            this.parent = parent;
        }

        public void OnPress()
        {
            if (parent != null) parent.OnPress();
        }
    }
}
