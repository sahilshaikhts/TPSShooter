using UnityEngine;

namespace Behaviourtree
{
public class Invertor : Node
{
    public Invertor(BehaviourTree aTree) : base(aTree) { }

    public override NodeState Execute()
    {
        return m_childNodes[0].Execute()==NodeState.Sucessful? NodeState.Sucessful : NodeState.Failed;
    }

    public override void AddChildNode(Node aChild)
    {
        if (m_childNodes.Count > 0)
            Debug.LogError("Can't add more than one child node to an invertor type node.");
        else m_childNodes.Add(aChild);
    }
}
}