using Behaviourtree;
using System;

/// <summary>
///This class is a generic condition node.
///In the Counstructor pass a reference of a method or a lamda expression that has return type bool.
/// </summary>
public class Condition : Node
{
    Func<bool> m_condition;

    public Condition(BehaviourTree aTree, Func<bool> aCondition): base(aTree)
    {
        m_condition = aCondition;
    }
    public override NodeState Execute(){return m_condition.Invoke() == true ? NodeState.Sucessful : NodeState.Failed;}
}
