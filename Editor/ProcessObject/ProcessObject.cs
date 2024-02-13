using System.Collections.Generic;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class ProcessObject
    {
        public enum DrawType
        {
            Normal,
            GimmickOnly, //Setされる側
            TriggerOnly, //Setする側
        }

        public DrawType drawType = DrawType.Normal;
        public bool folding = false;
        public GameObject gameObject;
        public List<Node> nodes = new List<Node>();
        
        public Entry[] entries;
        
        public bool hasParentGameObject = false;
        public bool hiding;

        public ObjectFrame objectFrame;
        
        public ProcessObject parent { get; private set; }
        public List<ProcessObject> children
        {
            get;
        } = new List<ProcessObject>();

        public void ResetDisplayState()
        {
            hiding = false;
            nodes.Clear();
            objectFrame = null;
        }

        public void SetParent(ProcessObject _parent)
        {
            parent = _parent;

            if (!parent.children.Contains(this))
                parent.AddChild(this);
        }

        public void AddChild(ProcessObject _child)
        {
            children.Add(_child);

            if (_child.parent != this)
                _child.SetParent(this);
        }

        public List<ProcessObject> GetAllChildren()
        {
            var allChildren = new List<ProcessObject>(children);
            foreach (var child in children)
            {
                allChildren.AddRange(child.GetAllChildren());
            }

            return allChildren;
        }

        public sealed class PutInfo
        {
            public Vector2 putEndPosition;
            public List<ProcessObject> putObjects = new List<ProcessObject>();
        }
    }
}
