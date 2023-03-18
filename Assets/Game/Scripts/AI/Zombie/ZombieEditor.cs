#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Game.Scripts.AI.Zombie
{
    [CustomEditor(typeof(ZombieBT), true)]
    public class ZombieEditor : Editor
    {
        private ZombieBT _BtWayPoint;

        private void Awake()
        {
            _BtWayPoint = (ZombieBT)target;
        }

        private void OnSceneGUI()
        {
            Handles.color = Color.blue;

            for (var i = 0; i < _BtWayPoint.WayPoints.Length; ++i)
            {
                _BtWayPoint.WayPoints[i].position =
                    Handles.PositionHandle(_BtWayPoint.WayPoints[i].position, Quaternion.identity);
                Handles.DrawLine(_BtWayPoint.WayPoints[i].position,
                    _BtWayPoint.WayPoints[(int)Mathf.Repeat(i + 1, _BtWayPoint.WayPoints.Length)].position, 2f);
            }
        }
    }
}

#endif