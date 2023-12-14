namespace CCKProcessTracer.Editor
{
    public sealed class DisplayUpdater
    {
        public static void Update()
        {
            DrawManager.ClearDrawObjects();

            var objects = ProcessObjectFactory.processObjects;
            foreach (var o in objects)
            {
                o.ResetDisplayState();
            }
            var nodes = NodeFactory.Create(objects);
            var connects = ConnectFactory.Create(nodes);

            ProcessObjectsAligner.Align(objects);
            ConnectAligner.Align(connects);
        }
    }
}
