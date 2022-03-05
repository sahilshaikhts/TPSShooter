using UnityEngine;

namespace Behaviourtree
{
public class Sequence : Node
{
    public Sequence(BehaviourTree aTree) : base(aTree) { }

    public override NodeState Execute()
    {
        if (m_currentChildIndex == -1) 
                m_currentChildIndex=0;

        switch (m_childNodes[m_currentChildIndex].Execute())
        {
            case NodeState.Sucessful:
                m_state = NodeState.Sucessful;
                m_currentChildIndex++;
                break;
            case NodeState.Failed:
                m_state = NodeState.Failed;
                break;

            case NodeState.Running:
                m_state = NodeState.Running;
                break;
        }

        if (m_currentChildIndex >= m_childNodes.Count) m_currentChildIndex = -1;

        return m_state;
    }

}
}