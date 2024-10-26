using _Project.Scripts.Common;
using Vector3 = UnityEngine.Vector3;

namespace DungeonCrawler._Project.Scripts.Events
{
    public class GridClickedEvent : IEvent
    {
        public Vector3 Position;
    }
}