using Game.Scripts.AI.BT.Core;
using UnityEngine;

namespace Game.Scripts.AI.Zombie.Action
{
    public class CheckEnemyInRange : Node
    {
        #region 애니메이션 캐시 변수

        private static readonly int ShouldMove = Animator.StringToHash("ShouldMove");
        private static readonly int CanAttack = Animator.StringToHash("CanAttack");

        #endregion

        #region 필수 변수

        private readonly Zombie _Owner;
        private readonly Animator _Animator;

        private readonly Transform _Transform;

        #endregion

        #region 속성

        private const int ENEMY_LAYER_MASK = 1 << 6;

        #endregion

        public CheckEnemyInRange(Transform transform) : base("Check Enemy")
        {
            _Owner = transform.GetComponent<ZombieBT>().Owner;
            _Animator = transform.GetComponent<Animator>();
            _Transform = transform;
        }

        public override NodeState Evaluate()
        {
            var colliders = Physics.OverlapSphere(
                _Transform.position, _Owner.Data.FOVRange, ENEMY_LAYER_MASK);


            if (colliders.Length > 0)
            {
                Vector3 dir = (colliders[0].transform.position - _Owner.SensorTransform.position).normalized;
                var dot = Vector3.Dot(dir, _Owner.SensorTransform.forward);

                if (dot < Mathf.Cos(_Owner.Data.FOV * 0.5f))
                {
                    State = NodeState.ENS_FAILURE;
                    return State;
                }

                Parent.Parent.SetData("Target", colliders[0].transform);
                _Animator.SetBool(ShouldMove, true);
                _Animator.SetBool(CanAttack, false);
                State = NodeState.ENS_SUCCESS;
                return State;
            }


            State = NodeState.ENS_FAILURE;
            return State;
        }
    }
}