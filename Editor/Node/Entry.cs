using ClusterVR.CreatorKit.Gimmick;
using ClusterVR.CreatorKit.Trigger;

namespace CCKProcessTracer.Editor
{
    public class Entry
    {
        public IGimmick gimmick;
        public ITrigger trigger;
        
        public Entry(IGimmick gimmick, ITrigger trigger)
        {
            this.gimmick = gimmick;
            this.trigger = trigger;
        }
    }
}