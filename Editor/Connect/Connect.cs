namespace CCKProcessTracer.Editor
{
    public sealed class Connect
    {
        public Arrow arrow;

        public Key from;
        public Node to;

        public Connect(Key key, Node node)
        {
            from = key;
            to = node;
        }
    }
}
