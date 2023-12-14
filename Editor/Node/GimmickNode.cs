using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class GimmickNode : Node
    {
        public GimmickNode(ProcessObject _processObject) : base(_processObject)
        {
        }

        protected override Vector2 PutNode(Vector2 position)
        {
            arrowReceivePosition = new Vector2(position.x, position.y + nameHeight * .5f);

            var b = new Button(this);
            b.rect = new Rect(position.x, position.y, nodeWidth, nameHeight);
            b.text = displayName;

            return new Vector2(position.x + nodeWidth, position.y + nameHeight);
        }
    }
}
