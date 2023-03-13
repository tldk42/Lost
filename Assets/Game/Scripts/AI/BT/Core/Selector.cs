using System.Collections.Generic;

namespace Game.Scripts.AI.BT.Core
{
    public class Selector : Node
    {
        public Selector()
        {
        }

        public Selector(List<Node> children) : base(children)
        {
        }

        public override NodeState Evaluate()
        {
            foreach (var node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.ENS_FAILURE:
                        continue;
                    case NodeState.ENS_SUCCESS:
                        State = NodeState.ENS_SUCCESS;
                        return State;
                    case NodeState.ENS_RUNNING:
                        State = NodeState.ENS_RUNNING;
                        return State;
                    default:
                        continue;
                }
            }

            State = NodeState.ENS_FAILURE;
            return State;
        }
    }
}