using Game.Scripts.AI.BT.Core;
using UnityEngine;

namespace Game.Scripts.AI.Zombie
{
    public class CheckAttackRange : Node
    {
        #region 애니메이션 캐시 변수

        private static readonly int ShouldMove = Animator.StringToHash("ShouldMove");
        private static readonly int CanAttack = Animator.StringToHash("CanAttack");

        #endregion

        #region 필수 변수

        private readonly ZombieBT _Owner;
        private readonly Animator _Animator;

        private readonly Transform _Transform;
        private readonly Transform[] _WayPoints;

        #endregion

        public CheckAttackRange(Transform transform) : base("Check Attack Range")
        {
            _Transform = transform;
            _Owner = transform.GetComponent<ZombieBT>();
            _Animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            var targetPlayer = GetData("Target");

            if (targetPlayer == null)
            {
                State = NodeState.ENS_FAILURE;
                return State;
            }

            Vector3 targetPosition = ((Transform)targetPlayer).position;

            if (Vector3.Distance(_Transform.position, targetPosition)
                <= _Owner.AttackRange)
            {
                _Animator.SetBool(CanAttack, true);
                _Animator.SetBool(ShouldMove, false);

                State = NodeState.ENS_SUCCESS;
                return State;
            }

            State = NodeState.ENS_FAILURE;
            return State;
        }
    }
}