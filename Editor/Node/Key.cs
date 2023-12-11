using System.Collections.Generic;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public class Key : IButton
    {

        public enum Target
        {
            ownItem,
            specifiedItem,
            global,
            player,
            collidedItemOrPlayer,
        }

        public enum ValueType
        {
            intType,
            floatType,
            doubleType,
            signalType,
            vector2Type,
            vector3Type,
        }
        public List<Node> afterNodes = new List<Node>();

        public Vector2 arrowSendPosition;
        public List<Connect> connects = new List<Connect>();
        public bool highlighted = false;

        public string keyName;
        public Target target;
        public GameObject targetObject;

        public Key(string name, ProcessObject _processObject, Node _node)
        {
            keyName = name;

            processObject = _processObject;
            node = _node;
        }
        public ProcessObject processObject
        {
            get;
        }
        public Node node
        {
            get;
        }

        public void OnPress()
        {
            ProcessHighlighter.Highlight(this);
        }
    }
}
