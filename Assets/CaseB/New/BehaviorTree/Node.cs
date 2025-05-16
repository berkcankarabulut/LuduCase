namespace CaseB.New.BehaviorTree
{
    public abstract class Node
    {
        protected string name; 
        protected Node(string name = "Node")
        {
            this.name = name;
        } 
        public abstract NodeStatus Execute();
    }
}