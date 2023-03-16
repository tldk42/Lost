using System.Collections.Generic;
using Game.Scripts.AI.BT.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.AI.Zombie
{
    public class ZombieBT : BehaviorTree
    {
        [FoldoutGroup("속성"), Range(0, 5)] public float Speed = 2f;
        [FoldoutGroup("속성"), Range(3, 10)] public float FOVRange = 6f;

        [Title("경로 설정")] [SerializeField, SceneObjectsOnly, GUIColor(0.37f, 0.52f, 0.64f, 1f)]
        private Transform[] WayPoints;

        protected override Node SetupTree() 
        {
            Node root = new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckEnemyInRange(transform),
                        new ChaseTarget(transform),
                    }),
                    new TaskPatrol(transform, WayPoints),
                }
            );
            return root;
        }
    }
}