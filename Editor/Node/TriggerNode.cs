using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class TriggerNode : Node
    {
        public TriggerNode(ProcessObject _processObject) : base(_processObject)
        {
        }

        protected override Vector2 PutNode(Vector2 position)
        {
            arrowReceivePosition = new Vector2(position.x, position.y + nameHeight * .5f);

            var b = new Button(this)
            {
                rect = new Rect(position.x, position.y, nodeWidth, nameHeight),
                text = displayName,
            };

            for (int i = 0; i < useKeys.Count; i++)
            {
                b = new Button(useKeys[i]);
                float keyYPos = nameHeight + nameKeyInterval + (keyHeight + keyInterval) * i;
                b.rect = new Rect(position.x, position.y + keyYPos, nodeWidth, keyHeight);
                b.text = useKeys[i].keyName;
                b.textColor = new Color(0.933f, 0.933f, 0.7f, 1.000f);

                useKeys[i].arrowSendPosition = new Vector2(position.x + nodeWidth, position.y + keyYPos + keyHeight * .5f);
            }

            return new Vector2(position.x + nodeWidth,
                position.y + nameHeight + nameKeyInterval + (keyHeight + keyInterval) * useKeys.Count);
        }
    }
}
