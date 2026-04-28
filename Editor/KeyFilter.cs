using System.Collections.Generic;

namespace CCKProcessTracer.Editor
{
    public static class KeyFilter
    {
        public static bool enabled;
        public static int selectedKeyIndex;
        public static string[] keyDisplayNames = { "All" };

        static readonly HashSet<Arrow> visibleArrows = new HashSet<Arrow>();
        static readonly HashSet<IButton> visibleButtonOwners = new HashSet<IButton>();
        static readonly HashSet<ProcessObject> visibleProcessObjects = new HashSet<ProcessObject>();

        public static bool IsActive => enabled && selectedKeyIndex > 0;

        public static string SelectedKeyName =>
            IsActive ? keyDisplayNames[selectedKeyIndex] : null;

        public static void CollectKeyNames(List<Node> nodes)
        {
            var keySet = new SortedSet<string>();
            foreach (var node in nodes)
            {
                foreach (var key in node.useKeys)
                {
                    if (!string.IsNullOrEmpty(key.keyName))
                        keySet.Add(ConnectFactory.TrimAxis(key.keyName));
                }
                if (!string.IsNullOrEmpty(node.receiveKeyName))
                    keySet.Add(ConnectFactory.TrimAxis(node.receiveKeyName));
            }

            var names = new List<string> { "All" };
            names.AddRange(keySet);
            keyDisplayNames = names.ToArray();

            if (selectedKeyIndex >= keyDisplayNames.Length)
                selectedKeyIndex = 0;
        }

        public static void BuildFilterSets()
        {
            visibleArrows.Clear();
            visibleButtonOwners.Clear();
            visibleProcessObjects.Clear();

            if (!IsActive) return;

            var targetKey = SelectedKeyName;

            foreach (var connect in ConnectFactory.connects)
            {
                if (ConnectFactory.TrimAxis(connect.from.keyName) == targetKey)
                {
                    if (connect.arrow != null)
                        visibleArrows.Add(connect.arrow);

                    visibleButtonOwners.Add(connect.from);
                    visibleButtonOwners.Add(connect.from.node);
                    visibleProcessObjects.Add(connect.from.processObject);

                    visibleButtonOwners.Add(connect.to);
                    visibleProcessObjects.Add(connect.to.processObject);
                }
            }

            foreach (var node in NodeFactory.nodes)
            {
                foreach (var key in node.useKeys)
                {
                    if (ConnectFactory.TrimAxis(key.keyName) == targetKey)
                    {
                        visibleButtonOwners.Add(key);
                        visibleButtonOwners.Add(node);
                        visibleProcessObjects.Add(node.processObject);
                    }
                }
                if (!string.IsNullOrEmpty(node.receiveKeyName) &&
                    ConnectFactory.TrimAxis(node.receiveKeyName) == targetKey)
                {
                    visibleButtonOwners.Add(node);
                    visibleProcessObjects.Add(node.processObject);
                }
            }
        }

        public static bool IsArrowVisible(Arrow arrow)
        {
            if (!IsActive) return true;
            return visibleArrows.Contains(arrow);
        }

        public static bool IsButtonVisible(IButton owner)
        {
            if (!IsActive) return true;
            if (owner is ObjectFrame frame)
                return visibleProcessObjects.Contains(frame.processObject);
            return visibleButtonOwners.Contains(owner);
        }

        public static bool IsFrameVisible(ProcessObject processObject)
        {
            if (!IsActive) return true;
            return visibleProcessObjects.Contains(processObject);
        }
    }
}
