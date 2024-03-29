using System.Collections.Generic;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class ProcessObjectsAligner
    {
        const float processObjectIntervalX = 20f;
        const float processObjectIntervalY = 30f;

        const float verticalMargin = 20f;
        const float horizontalMargin = 10f;

        const float typeOffsetX = 140f;

        static readonly Dictionary<int, Vector2> instanceIdDictionary = new Dictionary<int, Vector2>();
        static float lastPutPositionX; //親ごとに整理した際に、もっとも右に次のNodeを追加するための変数

        static void Reset()
        {
            instanceIdDictionary.Clear();
            lastPutPositionX = 0;
        }

        public static void Align(List<ProcessObject> objects)
        {
            Reset();
            var putStartPosition = new Vector2(30f, 50f);
            var notYetPut = new List<ProcessObject>(objects);

            while (notYetPut.Count > 0)
            {
                ProcessObject.PutInfo p = null;
                foreach (var o in notYetPut)
                {
                    if (o.parent == null)
                    {
                        p = PutProcessObject(o, putStartPosition);
                        break;
                    }
                }

                if (p == null)
                    return;

                foreach (var putObject in p.putObjects)
                {
                    if (notYetPut.Contains(putObject))
                        notYetPut.Remove(putObject);
                }
                putStartPosition.y = p.putEndPosition.y;
            }
        }

        static ProcessObject.PutInfo PutProcessObject(ProcessObject o, Vector2 putPosition)
        {
            var putInfo = new ProcessObject.PutInfo();
            putInfo.putObjects.Add(o);

            var nowPosition = putPosition;
            int instanceId = 0;
            if (o.parent == null) //親がある場合は適切にAlignされるので、親がない場合のみDrawTypeを考慮する
            {
                //親がない場合はHierarchy上での親ごとに並べる
                if (o.hasParentGameObject)
                {
                    instanceId = o.gameObject.transform.parent.GetInstanceID();
                }
                else
                {
                    instanceId = o.gameObject.GetInstanceID();
                }
                if (instanceIdDictionary.TryGetValue(instanceId, out var pos))
                {
                    nowPosition.x = pos.x;
                    nowPosition.y = pos.y + processObjectIntervalY;
                }
                else
                {
                    float x = 0f;
                    if (instanceIdDictionary.Values.Count > 0)
                    {
                        x = lastPutPositionX + processObjectIntervalX * 2 + typeOffsetX * 2;
                    }
                    nowPosition.x = x;
                    nowPosition.y = processObjectIntervalY;
                    instanceIdDictionary.Add(instanceId, nowPosition);
                }

                switch (o.drawType)
                {
                    case ProcessObject.DrawType.TriggerOnly:
                        nowPosition.x += 0;
                        break;
                    case ProcessObject.DrawType.Normal:
                        nowPosition.x += typeOffsetX;
                        break;
                    case ProcessObject.DrawType.GimmickOnly:
                        nowPosition.x += typeOffsetX * 2;
                        break;
                }
            }
            else
            {
                nowPosition.y += processObjectIntervalY;
            }

            var frameOriginPos = nowPosition;
            nowPosition.y += verticalMargin; // FrameとNodeの間に隙間を作る
            float maxXPosition = 0;

            if (o.folding)
            {
                var targets = o.GetAllChildren();
                putInfo.putObjects.AddRange(targets);
                foreach (var p in targets)
                {
                    p.hiding = true;
                }
            }
            else
            {
                var notYetPutNodes = new List<Node>(o.nodes);
                foreach (var node in o.nodes)
                {
                    var triggerNodePosition = node.PutNodeRecursive(new Vector2(nowPosition.x, nowPosition.y), notYetPutNodes);
                    nowPosition.y = triggerNodePosition.y;
                    nowPosition.y += Node.verticalNodeInterval;
                    maxXPosition = Mathf.Max(maxXPosition, triggerNodePosition.x);
                }

                var childPutPosition = new Vector2(maxXPosition, frameOriginPos.y);
                foreach (var child in o.children)
                {
                    var p = PutProcessObject(child,
                        childPutPosition + new Vector2(0, processObjectIntervalY));

                    putInfo.putObjects.AddRange(p.putObjects);

                    childPutPosition.y = p.putEndPosition.y;
                    maxXPosition = Mathf.Max(maxXPosition, p.putEndPosition.x);
                }
                
                nowPosition.x = maxXPosition;
                nowPosition.y = Mathf.Max(nowPosition.y, childPutPosition.y);
            }

            Rect rect;
            if (o.folding)
            {
                rect = new Rect(frameOriginPos.x, frameOriginPos.y, 0, 0);
            }
            else
            {
                rect = new Rect(frameOriginPos.x - horizontalMargin, frameOriginPos.y,
                    nowPosition.x - frameOriginPos.x - horizontalMargin, nowPosition.y - frameOriginPos.y);
            }

            var newFrame = new ObjectFrame(rect, o.gameObject, o);
            o.objectFrame = newFrame;
            if (o.parent == null)
            {
                instanceIdDictionary[instanceId] = new Vector2(instanceIdDictionary[instanceId].x, nowPosition.y);
                lastPutPositionX = Mathf.Max(nowPosition.x, lastPutPositionX);
            }
            putInfo.putEndPosition = new Vector2(nowPosition.x, nowPosition.y);
            return putInfo;
        }
    }
}
