using Behaviourtree;
using UnityEngine;

public class BoolCondition : Node
{
    private string key_BoolToCheck;
    private bool m_value=true;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aTree"></param>
    /// <param name="aBoolToCheck"></param>
    /// <param name="aTriggerValue">Value that will make this node return successful(Default:True)</param>
    public BoolCondition(BehaviourTree aTree, string aBoolToCheck,bool aTriggerValue=true) : base(aTree)
    {
        key_BoolToCheck = aBoolToCheck;
        m_value=aTriggerValue;
    }
    public override NodeState Execute()
    {
        Debug.Log(key_BoolToCheck +" : "+ (bool)GetData(key_BoolToCheck));
        if ((bool)GetData(key_BoolToCheck)== m_value)
        {
            return NodeState.Sucessful;
        }
        else
        {
            return NodeState.Failed;
        }
    }
}
