using Game.Scripts.AI.BT.Core;
using UnityEngine;

namespace Game.Scripts.AI.Zombie.Action
{
    public class ChaseTarget : Node
    {
        #region 필수 변수

        private readonly Zombie _Owner;

        private readonly Transform _Transform;

        #endregion

        public ChaseTarget( Transform transform) : base("Chase")
        {
            _Owner = transform.GetComponent<Zombie>();
            _Transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform targetTransform = (Transform)GetData("Target");
            Vector3 targetPosition = targetTransform.position;
            if (Vector3.Distance(_Transform.position, targetPosition) > 0.01f)
            {
                _Transform.position = Vector3.MoveTowards(
                    _Transform.position,
                    targetPosition,
                    _Owner.Data.Speed * Time.deltaTime);
                _Transform.LookAt(targetPosition);
            }

            State = NodeState.ENS_RUNNING;
            return State;
        }
    }
}