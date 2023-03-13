using System.Collections.Generic;

namespace Game.Scripts.AI.BT.Core
{
    public class Sequence : Node
    {
        public Sequence()
        {
        }

        public Sequence(List<Node> children) : base(children)
        {
        }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (var node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.ENS_FAILURE:
                        State = NodeState.ENS_FAILURE;
                        return State;
                    case NodeState.ENS_SUCCESS:
                        State = NodeState.ENS_SUCCESS;
                        continue;
                    case NodeState.ENS_RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        State = NodeState.ENS_SUCCESS;
                        return State;
                }
            }

            State = anyChildIsRunning ? NodeState.ENS_RUNNING : NodeState.ENS_SUCCESS;
            return State;
        }
    }
}