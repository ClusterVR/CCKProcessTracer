using System.Collections.Generic;
using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class RefObject
    {
        public GameObject gameObject;
        public List<Node> nodes = new List<Node>();
        public ObjectFrame objectFrame;
        public Vector2 dragOffset;

        public void ResetDisplayState()
        {
            nodes.Clear();
            objectFrame = null;
        }
    }
}
