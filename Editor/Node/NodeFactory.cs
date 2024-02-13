using System.Collections.Generic;
using System.Reflection;
using ClusterVR.CreatorKit.Gimmick;
using ClusterVR.CreatorKit.Gimmick.Implements;
using ClusterVR.CreatorKit.Item.Implements;
using ClusterVR.CreatorKit.Trigger;

namespace CCKProcessTracer.Editor
{
    public sealed class NodeFactory
    {
        public const float nodeIntervalX = 30f;
        public const float nodeIntervalY = 30f;

        public static List<Node> nodes = new();

        public static List<Node> Create(List<ProcessObject> objects)
        {
            nodes.Clear();
            foreach (var processObject in objects)
            {
                foreach (var entry in processObject.entries)
                {
                    if (entry.trigger != null)
                    {
                        var node = CreateTriggerNode(processObject, entry.trigger);
                        if (entry.gimmick != null)
                        {
                            var gimmickNode = CreateGimmickNode(processObject, entry.gimmick, objects.ToArray());
                            node.childGimmickNode = gimmickNode;
                            nodes.Add(gimmickNode); //Arrow表示のため別途Node一覧へは登録する
                        }
                        processObject.nodes.Add(node);
                        nodes.Add(node);
                    }
                    else if (entry.gimmick != null)
                    {
                        var node = CreateGimmickNode(processObject, entry.gimmick, objects.ToArray());
                        processObject.nodes.Add(node);
                        nodes.Add(node);
                    }
                }
            }
            return nodes;
        }

        static GimmickNode CreateGimmickNode(ProcessObject targetProcessObject, IGimmick gimmick, ProcessObject[] processObjects)
        {
            var node = new GimmickNode(targetProcessObject);
            node.displayName = gimmick.Key + "(" + gimmick.GetType().Name + ")";
            node.receiveKeyName = gimmick.Key;
            
            if (gimmick.Target == GimmickTarget.Global)
            {
                node.receiveTargetType = Node.ReceiveTarget.global;
            }
            else if (gimmick.Target == GimmickTarget.Player)
            {
                node.receiveTargetType = Node.ReceiveTarget.player;
            }
            else if (gimmick.Target == GimmickTarget.Item)
            {
                node.receiveTargetType = Node.ReceiveTarget.ownItem;
                
                if (gimmick is IGlobalGimmick)
                {
                    node.receiveTargetType = Node.ReceiveTarget.ownItem;

                    foreach (var o in processObjects)
                    {
                        Item item = null;

                        var setMethod = gimmick.GetType()
                            .GetField("globalGimmickKey",
                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (setMethod != null)
                        {
                            var globalGimmickKey =
                                setMethod.GetValue(gimmick) as GlobalGimmickKey;
                            var setMethod2 = globalGimmickKey.GetType()
                                .GetField("item",
                                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            if (setMethod2 != null) 
                                item = setMethod2.GetValue(globalGimmickKey) as Item;
                        }

                        if (item && o.gameObject == item.gameObject)
                        {
                            node.receiveTarget = o;
                            node.receiveTargetType = Node.ReceiveTarget.specifiedItem;
                        }
                    }
                }
            }
            return node;
        }
        
        static TriggerNode CreateTriggerNode(ProcessObject processObject, ITrigger trigger)
        {
            var node = new TriggerNode(processObject);
            node.displayName = trigger.GetType().Name;
                        
            foreach (var p in trigger.TriggerParams)
            {
                var key = SetUpTriggerKey(processObject, node, p);
                node.useKeys.Add(key);
            }
            return node;
        }

        static Key SetUpTriggerKey(ProcessObject processObject, Node node, TriggerParam triggerParam)
        {
            var key = new Key(triggerParam.RawKey, processObject, node);
            if (triggerParam.Target == TriggerTarget.SpecifiedItem && triggerParam.SpecifiedTargetItem != null)
            {
                key.targetObject = triggerParam.SpecifiedTargetItem.gameObject;
                key.target = Key.Target.specifiedItem;
            }
            else if (triggerParam.Target == TriggerTarget.Item)
            {
                key.targetObject = node.processObject.gameObject;
                key.target = Key.Target.ownItem;
            }
            else if (triggerParam.Target == TriggerTarget.CollidedItemOrPlayer)
            {
                key.target = Key.Target.collidedItemOrPlayer;
            }
            else if (triggerParam.Target == TriggerTarget.Player)
            {
                key.target = Key.Target.player;
            }
            else if (triggerParam.Target == TriggerTarget.Global)
            {
                key.target = Key.Target.global;
            }

            return key;
        }
    }
}