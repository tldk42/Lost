using System.Collections.Generic;


// ReSharper disable once InvalidXmlDocComment
/**
 * 참고 문헌
 * https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
 */


namespace Game.Scripts.AI.BT.Core
{
    public enum NodeState
    {
        ENS_RUNNING,
        ENS_SUCCESS,
        ENS_FAILURE,
    }

    public class Node
    {
        /** 이 노드의 상태 */
        protected NodeState State;

        /** 부모 및 자식 노드 */
        public Node Parent;

        protected List<Node> Children = new List<Node>();

        private Dictionary<string, object> _DataContext = new Dictionary<string, object>();

        public Node()
        {
            Parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (var child in children)
            {
                _Attach(child);
            }
        }

        private void _Attach(Node node)
        {
            node.Parent = this;
            Children.Add(node);
        }

        public void SetData(string key, object value)
        {
            _DataContext[key] = value;
        }

        public object GetData(string key)
        {
            if (_DataContext.TryGetValue(key, out var value))
            {
                return value;
            }

            Node node = Parent;

            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.Parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (_DataContext.ContainsKey(key))
            {
                _DataContext.Remove(key);
                return true;
            }

            Node node = Parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.Parent;
            }

            return false;
        }

        public Node GetRoot()
        {
            Node node = this;
            while (node.Parent != null)
            {
                node = node.Parent;
            }

            return node;
        }

        public virtual NodeState Evaluate() => NodeState.ENS_FAILURE;
    }
}