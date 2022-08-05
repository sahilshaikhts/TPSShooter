namespace Behaviourtree
{
public class ExecuteAll : Node
{
    public ExecuteAll(BehaviourTree aTree) : base(aTree) { }

    public override NodeState Execute()
    {
        foreach (Node node in m_childNodes)
        {
            node.Execute();
        }
        return NodeState.Sucessful;
    }

}
}