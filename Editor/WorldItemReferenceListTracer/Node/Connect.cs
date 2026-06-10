namespace CCKProcessTracer.Editor.WorldItemReferenceListTracer
{
    public sealed class Connect
    {
        public Key from;
        public Node to;
        public bool highlight = false;

        public Connect(Key from, Node to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
