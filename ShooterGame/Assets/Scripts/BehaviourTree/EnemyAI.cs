using Behaviourtree;

public class EnemyAI : BehaviourTree
{
    public void InitializeNodes()
    {
        m_root = new Selector(this);
        //MoveInRange
        Selector moveInRange = new Selector(this);
        IsWithinRange withingRangeNode = new IsWithinRange(this, "SelfPosition", "TargetPostion");
        MoveTowards moveTowardsNode = new MoveTowards(this, "ownerGO","TargetPostion");

        moveInRange.AddChildNode(withingRangeNode);
        moveInRange.AddChildNode(moveTowardsNode);
    }
}
