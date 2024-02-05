using System.Collections.Generic;
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
        
        public GimmickNode childGimmickNode;

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
            var existChildGimmickNode = childGimmickNode != null;
            if (existChildGimmickNode || useKeys.Count == 0) // Node内にGimmickがある場合はArrowが見えづらいため横にずらす
            {
                nowPosition.x += nodeWidth + NodeFactory.nodeIntervalX;
            }
            var putPosition = PutNode(nowPosition);
            nowPosition.y = putPosition.y;
            notYetPutNodes.Remove(this);
            if (existChildGimmickNode)
            {
                var childPosition = childGimmickNode.PutNode(nowPosition);
                nowPosition.y = childPosition.y;
            }
            nowPosition.x = putPosition.x + NodeFactory.nodeIntervalX;
            nowPosition.y += verticalNodeInterval;
            
            foreach (var key in useKeys)
            {
                foreach (var afterNode in key.afterNodes)
                {
                    if (afterNode.processObject == processObject && notYetPutNodes.Contains(afterNode))
                    {
                        putPosition = afterNode.PutNodeRecursive(nowPosition, notYetPutNodes);
                        nowPosition.y = Mathf.Max(putPosition.y, nowPosition.y);
                        nowPosition.y += verticalNodeInterval;
                    }
                }
            } 
            nowPosition.x = Mathf.Max(putPosition.x, nowPosition.x);
            nowPosition.y = Mathf.Max(putPosition.y, nowPosition.y);
            
            return nowPosition;
        }
    }
}
