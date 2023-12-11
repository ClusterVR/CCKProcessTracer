using System.Collections.Generic;
namespace CCKProcessTracer.Editor
{
    public class ProcessHighlighter
    {
        public static void ClearHighlights(List<Connect> connects, List<Node> nodes)
        {
            foreach (var connect in connects)
            {
                connect.arrow.highlight = false;
            }

            foreach (var n in nodes)
            {
                n.highlighted = false;
                foreach (var k in n.useKeys)
                {
                    k.highlighted = false;
                }
            }
        }

        public static void Highlight(object nodeOrKey)
        {
            ClearHighlights(ConnectFactory.connects, NodeFactory.nodes);

            if (nodeOrKey is TriggerNode)
                HighlightAfterProcessRecursive(nodeOrKey);
            else if (nodeOrKey is Node)
                HighlightBeforeProcessRecursive(nodeOrKey);

            if (nodeOrKey is Key)
                HighlightAfterProcessRecursive(nodeOrKey);
        }

        static void HighlightBeforeProcessRecursive(object nodeOrKey)
        {
            if (nodeOrKey is Node)
            {
                var node = nodeOrKey as Node;

                foreach (var connect in node.connects)
                {
                    if (connect.arrow.highlight)
                        continue;

                    connect.arrow.highlight = true;

                    HighlightBeforeProcessRecursive(connect.from);
                }
                node.highlighted = true;
            }

            if (nodeOrKey is Key)
            {
                var key = nodeOrKey as Key;

                key.highlighted = true;

                if (!key.node.highlighted)
                    HighlightBeforeProcessRecursive(key.node);
            }
        }

        static void HighlightAfterProcessRecursive(object nodeOrKey)
        {
            if (nodeOrKey is Node)
            {
                var node = nodeOrKey as Node;

                node.highlighted = true;

                foreach (var useKey in node.useKeys)
                {
                    if (!useKey.highlighted)
                        HighlightAfterProcessRecursive(useKey);
                }
            }

            if (nodeOrKey is Key)
            {
                var key = nodeOrKey as Key;

                foreach (var connect in key.connects)
                {
                    if (connect.arrow.highlight)
                        continue;

                    connect.arrow.highlight = true;

                    HighlightAfterProcessRecursive(connect.to);
                }
                key.highlighted = true;
            }
        }

        //引数に渡したノードかキーに繋がってる矢印全てのハイライトフラグを立てる関数
        static void HighlightProcessRecursive(object nodeOrKey)
        {
            if (nodeOrKey is Node)
            {
                var node = nodeOrKey as Node;

                foreach (var connect in node.connects)
                {
                    if (connect.arrow.highlight)
                        continue;

                    connect.arrow.highlight = true;

                    HighlightProcessRecursive(connect.from);
                }

                node.highlighted = true;

                foreach (var useKey in node.useKeys)
                {
                    if (!useKey.highlighted)
                        HighlightProcessRecursive(useKey);
                }
            }

            if (nodeOrKey is Key)
            {
                var key = nodeOrKey as Key;

                foreach (var connect in key.connects)
                {
                    if (connect.arrow.highlight)
                        continue;

                    connect.arrow.highlight = true;

                    HighlightProcessRecursive(connect.to);
                }

                key.highlighted = true;

                if (!key.node.highlighted)
                    HighlightProcessRecursive(key.node);
            }
        }
    }
}
