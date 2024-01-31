using System.Collections.Generic;
using ClusterVR.CreatorKit.Gimmick;
using ClusterVR.CreatorKit.Trigger;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class ProcessObject
    {

        public enum DrawType
        {
            Normal,
            SetterOnly,
            GetterOnly,
        }

        public DrawType drawType = DrawType.Normal;
        public bool folding = false;
        public GameObject gameObject;
        public List<Node> nodes = new List<Node>();
        
        public ITrigger[] triggers;
        public IGimmick[] gimmicks;
        
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

        public bool ContainsEntry(IGimmick entry)
        {
            foreach (var n in nodes)
            {
                if (n.gimmickEntry == entry)
                    return true; 
            }
            return false;
        }
        
        public bool ContainsEntry(ITrigger entry)
        {
            foreach (var n in nodes)
            {
                if (n.triggerEntry == entry)
                    return true; 
            }
            return false;
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
