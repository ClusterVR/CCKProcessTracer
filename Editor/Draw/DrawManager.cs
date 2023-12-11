namespace CCKProcessTracer.Editor
{
    public class DrawManager
    {
        public static void ClearDrawObjects()
        {
            ButtonDrawer.Clear();
            ArrowDrawer.Clear();
            ObjectFrameDrawer.Clear();
        }

        public static void Draw()
        {
            ArrowDrawer.DrawNormalArrow();
            ButtonDrawer.Draw();
            ArrowDrawer.DrawHighlightAllow();
            ObjectFrameDrawer.Draw();
        }
    }
}
