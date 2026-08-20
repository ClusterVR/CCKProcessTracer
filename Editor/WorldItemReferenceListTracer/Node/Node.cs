using System.Collections.Generic;
using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class Node : IButton
    {
        public RefObject refObject;
        public string displayName;
        public Rect rect;
        public List<Key> useKeys = new List<Key>();

        public const float nameHeight = 50f;
        public const float keyHeight = 25f;
        public const float nodeWidth = 220f;
        public const float verticalNodeInterval = 20f;

        public Node(RefObject refObject, string displayName)
        {
            this.refObject = refObject;
            this.displayName = displayName;
        }

        public Vector2 arrowReceivePosition
        {
            get
            {
                return new Vector2(rect.xMin, rect.yMin + nameHeight * 0.5f);
            }
        }

        public void OnPress()
        {
            if (refObject != null && refObject.gameObject != null)
            {
                UnityEditor.Selection.activeGameObject = refObject.gameObject;
                Debug.Log($"GameObject: {refObject.gameObject.name} | Script: {displayName}");
            }
        }

        public Vector2 PutNode(Vector2 position)
        {
            float totalHeight = nameHeight + keyHeight * useKeys.Count;
            this.rect = new Rect(position.x, position.y, nodeWidth, totalHeight);

            var b = new Button(this)
            {
                rect = new Rect(position.x, position.y, nodeWidth, nameHeight),
                text = displayName,
            };

            for (int i = 0; i < useKeys.Count; i++)
            {
                var key = useKeys[i];
                var keyBtn = new Button(key);
                float keyYPos = nameHeight + keyHeight * i;
                keyBtn.rect = new Rect(position.x, position.y + keyYPos, nodeWidth, keyHeight);
                keyBtn.text = key.GetDisplayName();
                
                key.arrowSendPosition = new Vector2(position.x + nodeWidth, position.y + keyYPos + keyHeight * 0.5f);
            }

            return new Vector2(position.x + nodeWidth, position.y + totalHeight);
        }
    }
}
