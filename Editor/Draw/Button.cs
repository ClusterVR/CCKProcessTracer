using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class Button
    {
        readonly IButton parent;
        public Rect rect;
        public string text;
        public Color textColor = new Color(0.933f, 0.933f, 0.933f, 1.000f);

        public Button(IButton parent, int displayOrder = -1)
        {
            if (displayOrder == -1)
                ButtonDrawer.buttons.Add(this);
            else
                ButtonDrawer.buttons.Insert(displayOrder, this);

            this.parent = parent;
        }

        public void OnPress()
        {
            if (parent != null)
                parent.OnPress();
        }
    }
}
