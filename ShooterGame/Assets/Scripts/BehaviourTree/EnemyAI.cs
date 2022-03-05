using Behaviourtree;
using UnityEngine;

public class EnemyAI : BehaviourTree
{
    [SerializeField]Transform m_playerTransform;

    public void Start()
    {
        InitializeBlackBoard();
        //MoveInRange
        Sequence moveInRange = new Sequence(this);
        m_root = moveInRange;
        IsWithinRange withingRangeNode = new IsWithinRange(this, "SelfPosition", "TargetPostion");
        MoveTowards moveTowardsNode = new MoveTowards(this, "ownerTransform","TargetPostion");

        moveInRange.AddChildNode(withingRangeNode);
        moveInRange.AddChildNode(moveTowardsNode);
    }

    void InitializeBlackBoard()
    {
        AddKey("SelfPosition");
        AddKey("TargetPostion");
        AddKey("ownerTransform",(Transform)this.transform);

    }

    private void Update()
    {
        SetData("SelfPosition", (Vector3)transform.position);
        SetData("TargetPostion", (Vector3)m_playerTransform.position);

        base.UpdateNodes();
    }
}
