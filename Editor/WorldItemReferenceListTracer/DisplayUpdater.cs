namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class DisplayUpdater
    {
        public static void Update()
        {
            ButtonDrawer.Clear();
            ObjectFrameDrawer.Clear();

            RefObjectFactory.RebuildAndAlign();
        }
    }
}
