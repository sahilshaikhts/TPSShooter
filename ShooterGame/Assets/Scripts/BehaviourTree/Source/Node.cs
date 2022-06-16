using System.Collections.Generic;

namespace Behaviourtree
{
public abstract class Node
{
    protected BehaviourTree m_tree;
    protected List<Node> m_childNodes;
    protected Node m_parentNode;
    protected NodeState m_state;

    public Node(BehaviourTree aTree) { m_tree = aTree; m_childNodes = new List<Node>(); }
    public abstract NodeState Execute();

    protected object GetData(string aKey)
    {
        if (m_tree)
        {
            return m_tree.GetData(aKey);
        }
            return null;
    }
    public void SetParentNode(Node aParent) { m_parentNode = aParent; }
    public void SetBehaviourTree(BehaviourTree aTree) { m_tree = aTree; }

    public void AddChildNode(Node aChild) { m_childNodes.Add(aChild); aChild.SetParentNode(this); }
}


public enum NodeState
{
    Running,
    Sucessful,
    Failed
}
}