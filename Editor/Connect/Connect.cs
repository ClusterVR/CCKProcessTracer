namespace CCKProcessTracer.Editor
{
    public class Connect
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
