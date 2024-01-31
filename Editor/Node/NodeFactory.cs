using System.Collections.Generic;
using System.Reflection;
using ClusterVR.CreatorKit.Gimmick;
using ClusterVR.CreatorKit.Gimmick.Implements;
using ClusterVR.CreatorKit.Item.Implements;
using ClusterVR.CreatorKit.Operation;
using ClusterVR.CreatorKit.Trigger;
using UnityEngine;

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
                foreach (var gimmick in processObject.gimmicks)
                {
                    if (processObject.ContainsEntry(gimmick))
                        continue;
                    var node = new GimmickNode(processObject);
                    node.displayName = gimmick.Key + "(" + gimmick.GetType().Name + ")";
                    node.receiveKeyName = gimmick.Key;
                    node.gimmickEntry = gimmick;

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

                            foreach (var o in objects)
                            {
                                Item item = null;

                                var setMethod = gimmick.GetType()
                                    .GetField("globalGimmickKey",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                if (setMethod != null)
                                {
                                    var globalGimmickKey =
                                        setMethod.GetValue(gimmick) as
                                            GlobalGimmickKey;

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
                    processObject.nodes.Add(node);
                    nodes.Add(node);
                }
                
                foreach (var trigger in processObject.triggers)
                {
                    if (processObject.ContainsEntry(trigger))
                        continue;
                    var node = new TriggerNode(processObject);
                    node.displayName = trigger.GetType().Name;
                    node.triggerEntry = trigger;

                    foreach (var p in trigger.TriggerParams)
                    {
                        var key = new Key(p.RawKey, processObject, node);

                        if (p.Target == TriggerTarget.SpecifiedItem && p.SpecifiedTargetItem != null)
                        {
                            key.targetObject = p.SpecifiedTargetItem.gameObject;
                            key.target = Key.Target.specifiedItem;
                        }
                        else if (p.Target == TriggerTarget.Item)
                        {
                            key.targetObject = node.processObject.gameObject;
                            key.target = Key.Target.ownItem;
                        }
                        else if (p.Target == TriggerTarget.CollidedItemOrPlayer)
                        {
                            key.target = Key.Target.collidedItemOrPlayer;
                        }
                        else if (p.Target == TriggerTarget.Player)
                        {
                            key.target = Key.Target.player;
                        }
                        else if (p.Target == TriggerTarget.Global)
                        {
                            key.target = Key.Target.global;
                        }
                        
                        node.useKeys.Add(key);
                    }
                    processObject.nodes.Add(node);
                    nodes.Add(node);
                }
                
            }
            return nodes;
        }
    }
}