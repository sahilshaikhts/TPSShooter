namespace Behaviourtree
{
public class Sequence : Node
{
    public Sequence(BehaviourTree aTree) : base(aTree) { }

    public override NodeState Execute()
    {
        foreach (Node node in m_childNodes)
        {
            switch (node.Execute())
            {
                case NodeState.Sucessful:
                    m_state = NodeState.Sucessful;
                    break;

                case NodeState.Failed:
                    m_state = NodeState.Failed;
                    return m_state;

                case NodeState.Running:
                    m_state = NodeState.Running;
                    return m_state;
            }
        }

        return m_state;
    }

}
}