using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public class Button
    {

        readonly IButton parent;
        public Rect rect;
        public string text;

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
