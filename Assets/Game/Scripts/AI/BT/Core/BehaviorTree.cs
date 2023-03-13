using UnityEngine;

namespace Game.Scripts.AI.BT.Core
{
    public abstract class BehaviorTree : MonoBehaviour
    {
        private Node _Root = null;
        [SerializeField] private Node _CurrentNode = null;

        protected void Start()
        {
            _Root = SetupTree();
            _CurrentNode = null;
        }

        private void Update()
        {
            _Root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}