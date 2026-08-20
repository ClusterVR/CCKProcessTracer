using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEditor;
using ClusterVR.CreatorKit.Item.Implements;

namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class RefObjectFactory
    {
        public static List<RefObject> refObjects = new List<RefObject>();
        public static List<Connect> connects = new List<Connect>();

        public static void Create()
        {
            refObjects.Clear();

            var allGo = Resources.FindObjectsOfTypeAll<GameObject>();
            var sceneGo = new List<GameObject>();
            foreach (var go in allGo)
            {
                if (go != null && AssetDatabase.GetAssetOrScenePath(go).Contains(".unity"))
                {
                    sceneGo.Add(go);
                }
            }

            foreach (var go in sceneGo)
            {
                var scriptableItem = go.GetComponent<ScriptableItem>();
                var playerScript = go.GetComponent<PlayerScript>();
                var refList = go.GetComponent<WorldItemReferenceList>();

                if (scriptableItem != null || playerScript != null || refList != null)
                {
                    var refObj = new RefObject();
                    refObj.gameObject = go;
                    refObjects.Add(refObj);
                }
            }

            refObjects.Sort((a, b) => CompareHierarchy(a.gameObject, b.gameObject));

            RebuildAndAlign();
        }

        public static void RebuildAndAlign()
        {
            connects.Clear();

            foreach (var refObj in refObjects)
            {
                refObj.nodes.Clear();

                var go = refObj.gameObject;
                if (go == null) continue;

                var scriptableItem = go.GetComponent<ScriptableItem>();
                var playerScript = go.GetComponent<PlayerScript>();
                var refList = go.GetComponent<WorldItemReferenceList>();

                if (scriptableItem != null)
                {
                    string scriptName = "ScriptableItem";
                    var setField = scriptableItem.GetType().GetField("sourceCodeAsset", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (setField != null)
                    {
                        var asset = setField.GetValue(scriptableItem) as JavaScriptAsset;
                        if (asset != null) scriptName = asset.name + ".js";
                    }

                    var node = new Node(refObj, scriptName);
                    refObj.nodes.Add(node);

                    string code = scriptableItem.GetSourceCode(true);
                    ParseScriptReferences(node, code, refList);
                }

                if (playerScript != null)
                {
                    string scriptName = "PlayerScript";
                    var setField = playerScript.GetType().GetField("sourceCodeAsset", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (setField != null)
                    {
                        var asset = setField.GetValue(playerScript) as JavaScriptAsset;
                        if (asset != null) scriptName = asset.name + ".js";
                    }

                    var node = new Node(refObj, scriptName);
                    refObj.nodes.Add(node);

                    string code = playerScript.GetSourceCode(true);
                    ParseScriptReferences(node, code, refList);
                }
            }

            foreach (var refObj in refObjects)
            {
                foreach (var node in refObj.nodes)
                {
                    foreach (var key in node.useKeys)
                    {
                        if (key.targetObject != null)
                        {
                            var targetObj = refObjects.FirstOrDefault(o => o.gameObject == key.targetObject);
                            if (targetObj != null && targetObj.nodes.Count > 0)
                            {
                                var connect = new Connect(key, targetObj.nodes[0]);
                                key.connects.Add(connect);
                                connects.Add(connect);
                            }
                        }
                    }
                }
            }

            AlignObjects();
        }

        private static void ParseScriptReferences(Node node, string code, WorldItemReferenceList refList)
        {
            if (string.IsNullOrEmpty(code)) return;

            var methodPattern = new Regex(@"[\$_]\.worldItemReference\(\s*[""']([^""']+)[""']\s*\)");
            var propPattern = new Regex(@"[\$_]\.worldItemReference\.([a-zA-Z_0-9]+)");

            var keys = new HashSet<string>();
            foreach (Match match in methodPattern.Matches(code))
            {
                keys.Add(match.Groups[1].Value);
            }
            foreach (Match match in propPattern.Matches(code))
            {
                keys.Add(match.Groups[1].Value);
            }

            foreach (var keyName in keys)
            {
                string targetName = "None";
                GameObject targetGo = null;

                if (refList != null)
                {
                    var entry = refList.WorldItemReferences.FirstOrDefault(e => e.Id == keyName);
                    if (entry != null && entry.Item != null)
                    {
                        targetGo = entry.Item.gameObject;
                        targetName = targetGo.name;
                    }
                }

                var key = new Key(node, keyName, targetName, targetGo);
                node.useKeys.Add(key);
            }
        }

        private static void AlignObjects()
        {
            var layers = new Dictionary<RefObject, int>();
            foreach (var obj in refObjects)
            {
                layers[obj] = 0;
            }

            bool changed = true;
            int maxIterations = refObjects.Count * 2;
            int iter = 0;
            while (changed && iter < maxIterations)
            {
                changed = false;
                iter++;
                foreach (var connect in connects)
                {
                    if (connect == null || connect.from == null || connect.to == null) continue;

                    var fromObj = refObjects.FirstOrDefault(o => o.nodes.Contains(connect.from.node));
                    var toObj = connect.to.refObject;

                    if (fromObj != null && toObj != null)
                    {
                        int targetLayer = layers[fromObj] + 1;
                        if (layers[toObj] < targetLayer)
                        {
                            if (targetLayer < 20)
                            {
                                layers[toObj] = targetLayer;
                                changed = true;
                            }
                        }
                    }
                }
            }

            var layerGroups = new Dictionary<int, List<RefObject>>();
            foreach (var obj in refObjects)
            {
                int layer = layers[obj];
                if (!layerGroups.ContainsKey(layer))
                {
                    layerGroups[layer] = new List<RefObject>();
                }
                layerGroups[layer].Add(obj);
            }

            float startX = 30f;
            float startY = 50f;
            float colWidth = 350f;
            float rowMargin = 40f;

            foreach (var pair in layerGroups)
            {
                int layer = pair.Key;
                var objsInLayer = pair.Value;

                float currentY = startY;
                float currentX = startX + layer * colWidth;

                foreach (var obj in objsInLayer)
                {
                    Vector2 pos = new Vector2(currentX, currentY) + obj.dragOffset;

                    float nodeY = pos.y + 20f;
                    float maxX = pos.x;
                    float maxY = nodeY;

                    foreach (var node in obj.nodes)
                    {
                        var nodeEnd = node.PutNode(new Vector2(pos.x, nodeY));
                        maxX = Mathf.Max(maxX, nodeEnd.x);
                        maxY = Mathf.Max(maxY, nodeEnd.y);
                        nodeY = nodeEnd.y + Node.verticalNodeInterval;
                    }

                    float frameHeight = (maxY - pos.y) + 10f;
                    Rect frameRect = new Rect(pos.x - 10f, pos.y, (maxX - pos.x) + 20f, frameHeight);
                    if (obj.nodes.Count == 0)
                    {
                        frameRect = new Rect(pos.x - 10f, pos.y, 100f, 40f);
                        frameHeight = 40f;
                    }

                    obj.objectFrame = new ObjectFrame(frameRect, obj.gameObject, obj);

                    currentY += frameHeight + rowMargin;
                }
            }
        }

        private static int CompareHierarchy(GameObject a, GameObject b)
        {
            if (a == b) return 0;
            if (a == null) return 1;
            if (b == null) return -1;

            var pathA = GetTransformPath(a.transform);
            var pathB = GetTransformPath(b.transform);

            int minLength = Mathf.Min(pathA.Count, pathB.Count);
            for (int i = 0; i < minLength; i++)
            {
                if (pathA[i] != pathB[i])
                {
                    return pathA[i].GetSiblingIndex().CompareTo(pathB[i].GetSiblingIndex());
                }
            }

            return pathA.Count.CompareTo(pathB.Count);
        }

        private static List<Transform> GetTransformPath(Transform t)
        {
            var path = new List<Transform>();
            var current = t;
            while (current != null)
            {
                path.Add(current);
                current = current.parent;
            }
            path.Reverse();
            return path;
        }
    }
}
