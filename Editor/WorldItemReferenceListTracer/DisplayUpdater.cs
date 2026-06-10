namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class DisplayUpdater
    {
        public static void Update()
        {
            ButtonDrawer.Clear();
            ObjectFrameDrawer.Clear();

            foreach (var obj in RefObjectFactory.refObjects)
            {
                obj.ResetDisplayState();
            }

            RefObjectFactory.RebuildAndAlign();
        }
    }
}
