using Game.Scripts.AI.BT;
using Game.Scripts.AI.BT.Core;

namespace Game.Scripts.AI.Zombie
{
    public class ZombieBT : BehaviorTree
    {
        public UnityEngine.Transform[] WayPoints;

        public float Speed = 2f;
        public float FOVRange = 6f;

        protected override Node SetupTree()
        {
            Node root = new TaskPatrol(transform, WayPoints);
            return root;
        }
    }
}