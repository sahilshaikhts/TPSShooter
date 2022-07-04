using Behaviourtree;
using System;

/// <summary>
///This class is a generic action node.
///In the Counstructor pass a reference of a method or a lamda expression that you want to execute.
/// </summary>
public class Task : Node
{
    Action m_task;

    public Task(BehaviourTree aTree, Action aTask) : base(aTree)
    {
        m_task = aTask;
    }
    public override NodeState Execute()
    {
        m_task.Invoke();
        return NodeState.Sucessful;
    }
}
