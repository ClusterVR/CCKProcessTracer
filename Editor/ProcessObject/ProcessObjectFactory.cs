using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Gimmick;
using ClusterVR.CreatorKit.Trigger;
using UnityEditor;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public sealed class ProcessObjectFactory
    {
        public static List<ProcessObject> processObjects = new List<ProcessObject>();
        public static bool displaySelectionOnly;

        public static void Create()
        {
            processObjects.Clear();
            SearchGimmicksAndCreate();
            processObjects.Sort((a, b) => CompareHierarchy(a.gameObject, b.gameObject));
            SetParentToProcessObjects();
        }

        static void SearchGimmicksAndCreate()
        {
            GameObject[] allGameObjects;
            if (displaySelectionOnly)
            {
                allGameObjects = Selection.gameObjects;
            }
            else
            {
                allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            }

            var allGameObjectsInScene = new List<GameObject>();
            foreach (var go in allGameObjects)
            {
                if (AssetDatabase.GetAssetOrScenePath(go).Contains(".unity"))
                    allGameObjectsInScene.Add(go);
            }

            while (allGameObjectsInScene.Any())
            {
                var createByGimmickGameObjects = new List<GameObject>();

                foreach (var g in allGameObjectsInScene)
                {
                    var tmpGimmicks = g.GetComponents<IGimmick>();
                    var tmpTriggers = g.GetComponents<ITrigger>();
                    
                    var tmpCreateItemGimmick =
                        g.GetComponents<ICreateItemGimmick>();

                    bool alreadyAssigned = false;
                    foreach (var p in processObjects)
                    {
                        if (p.gameObject == g) alreadyAssigned = true;
                    }
                    
                    if ((tmpGimmicks.Any() || tmpTriggers.Any()) && !alreadyAssigned)
                    {
                        var processObject = new ProcessObject();
                        processObject.gameObject = g;
                        processObject.hasParentGameObject = g.transform.parent != null;
                        processObject.entries = CreateEntries(tmpGimmicks, tmpTriggers);
                        
                        //GameObject内にGimmickかTriggerどちらかしかない場合はArrowが見えづらいためGameObjectの表示を1階層ずらす
                        if (!tmpGimmicks.Any())
                        {
                            processObject.drawType = ProcessObject.DrawType.TriggerOnly;
                        }
                        if (!tmpTriggers.Any())
                        {
                            processObject.drawType = ProcessObject.DrawType.GimmickOnly;
                        }
                        processObjects.Add(processObject);
                        
                        foreach (var c in tmpCreateItemGimmick)
                        {
                            createByGimmickGameObjects.Add(c.ItemTemplate.gameObject);
                        }
                    }
                }
                allGameObjectsInScene = createByGimmickGameObjects;
            }
        }

        static void SetParentToProcessObjects()
        {
            foreach (var processObject in processObjects)
            {
                var target = processObject.gameObject.transform.parent;

                while (target)
                {
                    var parentProcessObject =
                        SearchProcessObjectByGameObject(target.gameObject);

                    if (parentProcessObject != null)
                    {
                        processObject.SetParent(parentProcessObject);
                        break;
                    }

                    target = target.parent;
                }
            }
        }

        static ProcessObject SearchProcessObjectByGameObject(GameObject searchTarget)
        {
            foreach (var processObject in processObjects)
            {
                if (processObject.gameObject == searchTarget)
                    return processObject;
            }
            return null;
        }

        static Entry[] CreateEntries(IGimmick[] gimmicks, ITrigger[] triggers)
        {
            var dict = new Dictionary<Component,(IGimmick, ITrigger)>();
            foreach (var gimmick in gimmicks)
            {
                dict[(Component)gimmick] = (gimmick, null);
            }
            foreach (var trigger in triggers)
            {
                if (dict.ContainsKey((Component)trigger))
                {
                    var gimmick = dict[(Component)trigger].Item1;
                    dict[(Component)trigger] = (gimmick, trigger);
                }
                else
                {
                    dict[(Component)trigger] = (null, trigger);
                }
            }
            return dict.Values.Select(v => new Entry(v.Item1, v.Item2)).ToArray();
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
