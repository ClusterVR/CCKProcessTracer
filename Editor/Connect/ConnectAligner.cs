using System.Collections.Generic;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public class ConnectAligner : MonoBehaviour
    {
        public static void Align(List<Connect> connects)
        {
            foreach (var connect in connects)
            {
                var from = connect.from.arrowSendPosition;
                var to = connect.to.arrowReceivePosition;

                if (connect.to.processObject.hiding)
                {
                    var target = connect.to.processObject.parent;

                    while (target != null)
                    {
                        if (!target.hiding)
                        {
                            to = target.objectFrame.arrowReceivePosition;
                            break;
                        }

                        target = target.parent;
                    }
                }
                else if (connect.to.processObject.folding)
                {
                    to = connect.to.processObject.objectFrame.arrowReceivePosition;
                }

                if (connect.from.processObject.hiding)
                {
                    var target = connect.from.processObject.parent;

                    while (target != null)
                    {
                        if (!target.hiding)
                        {
                            from = target.objectFrame.arrowSendPosition;
                            break;
                        }

                        target = target.parent;
                    }
                }
                else if (connect.from.processObject.folding)
                {
                    from = connect.from.processObject.objectFrame.arrowSendPosition;
                }

                var a = new Arrow(from, to);
                connect.arrow = a;
            }
        }
    }
}
