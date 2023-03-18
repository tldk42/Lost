using Game.Scripts.AI.BT.Core;
using UnityEngine;

namespace Game.Scripts.AI.Zombie.Action
{
    public class TaskPatrol : Node
    {
        #region 애니메이션 캐시 변수

        private static readonly int ShouldMove = Animator.StringToHash("ShouldMove");

        #endregion

        #region 필수 변수

        private readonly Zombie _Owner;
        private readonly Animator _Animator;

        private readonly Transform _Transform;
        private readonly Transform[] _WayPoints;

        #endregion

        #region 속성

        private int _CurrentWaypointIndex = 0;
        private float _WaitTime = 1f;
        private float _WaitCounter = 0f;
        private bool _IsWaiting;

        #endregion

        public TaskPatrol(Transform transform, Transform[] wayPoints) : base("Patrol")
        {
            _Owner = transform.GetComponent<ZombieBT>().Owner;
            _Animator = transform.GetComponent<Animator>();
            _Transform = transform;
            _WayPoints = wayPoints;
        }

        /**
         * 1. 목표 지점에서 대기 상태 -> WaitTime만큼 대기
         * 2. 목표 지점에 도착 상태 -> 목표지점 Index + 1, Wait Flag 켜기
         * 3. 목표 지점에 가는 상태 -> 이동시키기
         */
        public override NodeState Evaluate()
        {
            if (_IsWaiting)
            {
                _WaitCounter += Time.deltaTime;
                if (_WaitCounter >= _WaitTime)
                {
                    _IsWaiting = false;
                    _Animator.SetBool(ShouldMove, true);
                }
            }
            else
            {
                Vector3 waypointPosition = _WayPoints[_CurrentWaypointIndex].position;
                if (Vector3.Distance(_Transform.position, waypointPosition) < 0.01f)
                {
                    _Transform.position = waypointPosition;
                    _WaitCounter = 0f;
                    _IsWaiting = true;

                    _CurrentWaypointIndex = (_CurrentWaypointIndex + 1) % _WayPoints.Length;

                    _Animator.SetBool(ShouldMove, false);
                }
                else
                {
                    _Transform.position = Vector3.MoveTowards(
                        _Transform.position,
                        waypointPosition,
                        _Owner.Data.Speed * Time.deltaTime);
                    _Animator.SetBool(ShouldMove, true);
                    _Transform.LookAt(waypointPosition);
                }
            }

            State = NodeState.ENS_RUNNING;
            return State;
        }
    }
}