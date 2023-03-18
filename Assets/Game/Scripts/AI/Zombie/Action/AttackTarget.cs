using Game.Scripts.AI.BT.Core;
using UnityEngine;

namespace Game.Scripts.AI.Zombie.Action
{
    public class AttackTarget : Node
    {
        #region 애니메이션 캐시 변수
        
        private static readonly int ShouldMove = Animator.StringToHash("ShouldMove");
        private static readonly int CanAttack = Animator.StringToHash("CanAttack");

        #endregion
        
        #region 필수 변수

        private readonly Zombie _Owner;
        private readonly Animator _Animator;

        private readonly Transform _Transform;
        private readonly Transform[] _WayPoints;

        #endregion
        
        public AttackTarget(Transform transform) : base("Attack Target")
        {
            _Transform = transform;
            _Owner = transform.GetComponent<ZombieBT>().Owner;
            _Animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            var targetPlayer = GetData("Target");
            if (targetPlayer == null)
            {
                State = NodeState.ENS_FAILURE;
                Debug.Log("목표물 없어짐");
                return State;
            }
            
            _Animator.SetBool(CanAttack, true);
            _Animator.SetBool(ShouldMove, false);

            State = NodeState.ENS_SUCCESS;
            return State;
        }
    }
}