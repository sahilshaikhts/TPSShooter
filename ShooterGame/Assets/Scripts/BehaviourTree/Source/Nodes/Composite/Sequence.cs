using UnityEngine;

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
                    break;
                case NodeState.Failed:
                    return NodeState.Failed;
                case NodeState.Running:
                    return NodeState.Running;
            }
        }
        return NodeState.Sucessful;
    }

}
}