namespace CaseB.New.BehaviorTree
{
    public class Action : Node
    {
        private System.Func<NodeStatus> _action; 
        public Action(System.Func<NodeStatus> action, string name = "Action") : base(name)
        {
            this._action = action;
        } 
        public override NodeStatus Execute()
        {
            return _action();
        }
    }
}