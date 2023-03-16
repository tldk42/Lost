using Game.Scripts.AI.BT.Core;
using UnityEngine;

namespace Game.Scripts.AI.Zombie
{
    public class CheckEnemyInRange : Node
    {
        #region 애니메이션 캐시 변수

        private static readonly int ShouldMove = Animator.StringToHash("ShouldMove");

        #endregion

        #region 필수 변수

        private readonly ZombieBT _Owner;
        private readonly Animator _Animator;

        private readonly Transform _Transform;

        #endregion

        #region 속성

        private const int ENEMY_LAYER_MASK = 1 << 6;

        #endregion

        public CheckEnemyInRange(Transform transform) : base("Check Enemy")
        {
            _Owner = transform.GetComponent<ZombieBT>();
            _Animator = transform.GetComponent<Animator>();
            _Transform = transform;
        }

        public override NodeState Evaluate()
        {
            var targetPlayer = GetData("Target");


            Collider[] colliders = Physics.OverlapSphere(
                _Transform.position, 6, ENEMY_LAYER_MASK);

            // Collider[] colliders = { };
            // Physics.OverlapSphereNonAlloc(_Transform.position, _Owner.FOVRange, colliders, ENEMY_LAYER_MASK);

            if (colliders.Length > 0)
            {
                Parent.Parent.SetData("Target", colliders[0].transform);
                _Animator.SetBool(ShouldMove, true);
                State = NodeState.ENS_SUCCESS;
                return State;
            }


            State = NodeState.ENS_FAILURE;
            return State;
        }
    }
}