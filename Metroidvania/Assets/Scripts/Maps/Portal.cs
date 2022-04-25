using UnityEngine;

namespace JlMetroidvaniaProject.Maps
{
    public class Portal : JlBehaviour
    {
        public int portalID;
        
        public Destination destination;
        public LatencyArea latency;
        public Teleporter teleporter;

        protected override void Awake()
        {
            teleporter.portalID = portalID;
        }
    }
}