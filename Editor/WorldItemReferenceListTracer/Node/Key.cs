using System.Collections.Generic;
using UnityEngine;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class Key : IButton
    {
        public Node node;
        public string keyName;
        public string targetName;
        public GameObject targetObject;

        public Vector2 arrowSendPosition;
        public List<Connect> connects = new List<Connect>();

        public Key(Node node, string keyName, string targetName, GameObject targetObject)
        {
            this.node = node;
            this.keyName = keyName;
            this.targetName = targetName;
            this.targetObject = targetObject;
        }

        public string GetDisplayName()
        {
            return $"{keyName} - {targetName}";
        }

        public void OnPress()
        {
            if (targetObject != null)
            {
                UnityEditor.Selection.activeGameObject = targetObject;
            }
        }
    }
}
