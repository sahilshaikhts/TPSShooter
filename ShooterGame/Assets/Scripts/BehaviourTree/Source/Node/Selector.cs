namespace Behaviourtree
{
public class Selector : Node
{
    public Selector(BehaviourTree aTree) : base(aTree) { }

    public override NodeState Execute()
    {
        foreach (Node node in m_childNodes)
        {
            switch (node.Execute())
            {
                case NodeState.Sucessful:
                    m_state = NodeState.Sucessful;
                        return m_state;
                case NodeState.Failed:
                    m_state = NodeState.Failed;
                    break;

                case NodeState.Running:
                    m_state = NodeState.Running;
                    break;
            }
        }
        return m_state;
    }

}
}