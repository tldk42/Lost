using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.AI.BT.Core
{
    public abstract class BehaviorTree : MonoBehaviour
    {
        [SerializeField, ReadOnly]private Node Root;

        protected void Start()
        {
            Root = SetupTree();
        }

        /** 매 프레임 자식 노드들을 evaluate */
        private void Update()
        {
            Root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}