using Behaviourtree;

public class EnemyAI : BehaviourTree
{
    public void InitializeNodes()
    {
        m_root = new Selector(this);
        //MoveInRange
        Selector moveInRange = new Selector(this);
        IsWithinRange withingRangeNode = new(this, "SelfPosition", "TargetPostion");
        MoveTowards withingRangeNode = new(this, "TargetPostion");
    }
}
