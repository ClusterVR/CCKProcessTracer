namespace CCKProcessTracer.Editor
{
    public sealed class DrawManager
    {
        public static void ClearDrawObjects()
        {
            ButtonDrawer.Clear();
            ArrowDrawer.Clear();
            ObjectFrameDrawer.Clear();
        }

        public static void Draw()
        {
            ObjectFrameDrawer.Draw();
            NodeDrawer.Draw();
            ArrowDrawer.DrawNormalArrow();
            ButtonDrawer.Draw();
            ArrowDrawer.DrawHighlightAllow();
        }
    }
}
