using System.Collections.Generic;

namespace Game.Scripts.AI.BT.Core
{
    public class Sequence : Node
    {

        public Sequence( List<Node> children) : base("Sequence", children)
        {
        }

        /** Sequence Node는 실패하면 BT의 상단으로 복귀 성공하면 다음 노드 평가 */
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