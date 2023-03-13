using Game.Scripts.AI.BT.Core;
using UnityEngine;

namespace Game.Scripts.AI.Zombie
{
    public class TaskPatrol : Node
    {
        private static readonly int ShouldMove = Animator.StringToHash("ShouldMove");

        private readonly ZombieBT _Owner;
        private readonly Animator _Animator;

        private readonly Transform _Transform;
        private readonly Transform[] _WayPoints;

        private int _CurrentWaypointIndex = 0;
        private readonly float _WaitTime = 1f;
        private float _WaitCounter = 0f;
        private bool _IsWaiting;

        public TaskPatrol(Transform transform, Transform[] wayPoints)
        {
            _Owner = transform.GetComponent<ZombieBT>();
            _Animator = transform.GetComponent<Animator>();
            _Transform = transform;
            _WayPoints = wayPoints;
        }

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
                Transform waypoint = _WayPoints[_CurrentWaypointIndex];
                if (Vector3.Distance(_Transform.position, waypoint.position) < 0.01f)
                {
                    _Transform.position = waypoint.position;
                    _WaitCounter = 0f;
                    _IsWaiting = true;

                    _CurrentWaypointIndex = (_CurrentWaypointIndex + 1) % _WayPoints.Length;

                    _Animator.SetBool(ShouldMove, false);
                }
                else
                {
                    var position = waypoint.position;
                    _Transform.position = Vector3.MoveTowards(
                        _Transform.position,
                        position,
                        _Owner.Speed * Time.deltaTime);
                    _Animator.SetBool(ShouldMove, true);
                    _Transform.LookAt(position);
                }
            }

            State = NodeState.ENS_RUNNING;
            return State;
        }
    }
}