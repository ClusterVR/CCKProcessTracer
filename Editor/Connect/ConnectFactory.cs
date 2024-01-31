using System.Collections.Generic;

namespace CCKProcessTracer.Editor
{
    public sealed class ConnectFactory
    {
        public static List<Connect> connects = new List<Connect>();

        public static List<Connect> Create(List<Node> nodes)
        {
            connects.Clear();
            foreach (var node in nodes)
            {
                foreach (var targetNode in nodes)
                {
                    foreach (var key in node.useKeys)
                    {
                        if (string.IsNullOrEmpty(key.keyName) || string.IsNullOrEmpty(targetNode.receiveKeyName))
                            continue;
                        var keyName = TrimAxis(key.keyName);
                        var targetKeyName = TrimAxis(targetNode.receiveKeyName); 
                        
                        if (keyName == targetKeyName)
                        {
                            if (key.target == Key.Target.ownItem)
                            {
                                if (targetNode.receiveTargetType == Node.ReceiveTarget.ownItem
                                    && node.processObject == targetNode.processObject)
                                {
                                    MakeConnect(key, targetNode);
                                }
                                else if (targetNode.receiveTargetType == Node.ReceiveTarget.specifiedItem
                                    && node.processObject == targetNode.receiveTarget)
                                {
                                    MakeConnect(key, targetNode);
                                }
                            }
                            else if (key.target == Key.Target.specifiedItem)
                            {
                                if (targetNode.receiveTargetType == Node.ReceiveTarget.ownItem
                                    && key.targetObject == targetNode.processObject.gameObject)
                                {
                                    MakeConnect(key, targetNode);
                                }
                                else if (targetNode.receiveTargetType == Node.ReceiveTarget.specifiedItem
                                    && key.targetObject == targetNode.receiveTarget.gameObject)
                                {
                                    MakeConnect(key, targetNode);
                                }
                            }
                            else if (key.target == Key.Target.player &&
                                targetNode.receiveTargetType == Node.ReceiveTarget.player)
                            {
                                MakeConnect(key, targetNode);
                            }
                            else if (key.target == Key.Target.global &&
                                targetNode.receiveTargetType == Node.ReceiveTarget.global)
                            {
                                MakeConnect(key, targetNode);
                            }
                            else if (key.target == Key.Target.collidedItemOrPlayer)
                            {
                                if (targetNode.receiveTargetType == Node.ReceiveTarget.ownItem)
                                {
                                    MakeConnect(key, targetNode);
                                }
                                else if (targetNode.receiveTargetType == Node.ReceiveTarget.specifiedItem)
                                {
                                    MakeConnect(key, targetNode);
                                }
                            }
                        }
                    }
                }
            }
            return connects;
        }

        static string TrimAxis(string key)
        {
            if (key.EndsWith(".x") || key.EndsWith(".y") || key.EndsWith(".z"))
            {
                key = key.Substring(0, key.Length - 2);
            }
            return key;
        }

        static void MakeConnect(Key key, Node node)
        {
            key.afterNodes.Add(node);
            node.beforeKeys.Add(key);
            var connect = new Connect(key, node);
            connects.Add(connect);

            key.connects.Add(connect);
            node.connects.Add(connect);
        }
    }
}
