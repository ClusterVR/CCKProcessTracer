using System.Collections.Generic;
using ClusterVR.CreatorKit.Gimmick;
using ClusterVR.CreatorKit.Operation;
using ClusterVR.CreatorKit.Trigger;
using UnityEditor;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public abstract class Node : IButton
    {

        public enum ReceiveTarget
        {
            ownItem,
            specifiedItem,
            player,
            global,
        }
        protected const float nameKeyInterval = 0f;
        protected const float keyInterval = 0f;
        protected const float keyHeight = 25f;
        protected const float nameHeight = 50f;
        protected const float nodeWidth = 200f;

        public const float verticalNodeInterval = 20f;
        public Vector2 arrowReceivePosition;
        
        public Component componentEntry;
        public ITrigger triggerEntry;
        public IGimmick gimmickEntry;
        public ILogic logicEntry;

        public List<Key> beforeKeys = new List<Key>();
        public List<Connect> connects = new List<Connect>();
        public string displayName;
        public bool highlighted = false;
        public ProcessObject processObject;
        public string receiveKeyName;
        public ProcessObject receiveTarget;

        public ReceiveTarget receiveTargetType;

        public List<Key> useKeys = new List<Key>();

        public Node(ProcessObject _processObject)
        {
            processObject = _processObject;
        }

        public void OnPress()
        {
            ProcessHighlighter.Highlight(this);
            Selection.activeGameObject = processObject.gameObject;
            Debug.Log($"GameObject: {processObject.gameObject.name}\r\nComponent: {displayName}");
        }

        protected abstract Vector2 PutNode(Vector2 position);

        public bool ContainsKey(Key key)
        {
            foreach (var k in this.useKeys)
            {
                if (k == key)
                    return true;
            }
            return false;
        }
        
        public Vector2 PutNodeRecursive(Vector2 nowPosition, List<Node> notYetPutNodes)
        {
            if (!notYetPutNodes.Contains(this))
                return nowPosition;


            var putPosition = PutNode(nowPosition);
            float tmpY = putPosition.y;
            nowPosition.x = putPosition.x + NodeFactory.nodeIntervalX;

            notYetPutNodes.Remove(this);

            foreach (var key in useKeys)
            {
                foreach (var afterNode in key.afterNodes)
                {
                    if (afterNode.processObject == processObject && notYetPutNodes.Contains(afterNode))
                    {
                        putPosition = afterNode.PutNodeRecursive(nowPosition, notYetPutNodes);


                        if (putPosition.y > nowPosition.y)
                            nowPosition.y = putPosition.y;
                        if (tmpY > nowPosition.y)
                            nowPosition.y = tmpY;

                        nowPosition.y += verticalNodeInterval;
                    }
                }
            }

            nowPosition.y -= verticalNodeInterval;

            if (putPosition.x > nowPosition.x)
                nowPosition.x = putPosition.x;

            if (putPosition.y > nowPosition.y)
                nowPosition.y = putPosition.y;


            return nowPosition;
        }
    }
}
