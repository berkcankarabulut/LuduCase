using System.Collections.Generic;

namespace CaseB.New.BehaviorTree
{
    public class Sequence : Node
    {
        private List<Node> _children = new List<Node>();
        private int _currentChild = 0;

        public Sequence(string name = "Sequence") : base(name) { }

        public Sequence AddChild(Node node)
        {
            _children.Add(node);
            return this;
        }

        public override NodeStatus Execute()
        { 
            if (_children.Count == 0)
                return NodeStatus.Success;
 
            NodeStatus childStatus = _children[_currentChild].Execute();

            switch (childStatus)
            {
                case NodeStatus.Running:
                    return NodeStatus.Running;

                case NodeStatus.Failure: 
                    _currentChild = 0;   
                    return NodeStatus.Failure;

                case NodeStatus.Success:
                    _currentChild++;
                    if (_currentChild < _children.Count) return NodeStatus.Running;
                    _currentChild = 0;   
                    return NodeStatus.Success;

                default:
                    return NodeStatus.Failure;
            }
        }
    }
}