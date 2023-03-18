using System;
using System.Collections.Generic;
using Game.Scripts.AI.BT.Core;
using Game.Scripts.AI.Zombie.Action;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.AI.Zombie
{
    public class ZombieBT : BehaviorTree
    {
        public Zombie Owner { get; private set; }

        [Title("경로 설정")] [SerializeField, SceneObjectsOnly, GUIColor(0.37f, 0.52f, 0.64f, 1f)]
        public Transform[] WayPoints;

        private void Awake()
        {
            Owner = transform.GetComponent<Zombie>();
        }

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
                {
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new CheckAttackRange(transform),
                            new AttackTarget(transform)
                        }),
                        new Sequence(new List<Node>     
                        {
                            new CheckEnemyInRange(transform),
                            new ChaseTarget(transform),
                        })
                    }),

                    new TaskPatrol(transform, WayPoints),
                }
            );
            return root;
        }
    }
}