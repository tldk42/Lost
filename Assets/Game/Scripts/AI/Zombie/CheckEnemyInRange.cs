using Game.Scripts.AI.BT.Core;
using UnityEngine;

namespace Game.Scripts.AI.Zombie
{
    public class CheckEnemyInRange : Node
    {
        
        // TODO: Animator를 노드마다 가질것인가?
        private static int _EnemyLayerMask = 1 << 6;

        private Transform _Transform;

        public CheckEnemyInRange(Transform transform)
        {
            _Transform = transform;
        }

        public override NodeState Evaluate()
        {
            object target = GetData("Target");
            if (target == null)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    _Transform.position, 6, _EnemyLayerMask);

                if (colliders.Length > 0)
                {
                    Parent.Parent.SetData("Target", colliders[0].transform);
                    // TODO: Animator 변수 상황에 맞게 변경
                    State = NodeState.ENS_SUCCESS;
                    return State;
                }
            }

            State = NodeState.ENS_FAILURE;
            return State;
        }
    }
}