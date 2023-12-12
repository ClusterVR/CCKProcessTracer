using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class Arrow
    {

        public Vector2 from;
        public bool highlight;
        public Vector2 to;

        public Arrow(Vector2 _from, Vector2 _to)
        {
            from = _from;
            to = _to;

            ArrowDrawer.arrows.Add(this);
        }
    }
}
