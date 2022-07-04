using Behaviourtree;

public class BoolCondition : Node
{
    private string key_BoolToCheck;

    public BoolCondition(BehaviourTree aTree, string aBoolToCheck): base(aTree)
    {
        key_BoolToCheck = aBoolToCheck;
    }
    public override NodeState Execute()
    {
        if ((bool)GetData(key_BoolToCheck)==true)
        {
            return NodeState.Sucessful;
        }
        else
        {
            return NodeState.Failed;
        }
    }
}
