using Behaviourtree;
using UnityEngine;

public class BT_luminousCentaurs : BehaviourTree
{
    [SerializeField] Transform m_playerTransform;

    public void Start()
    {
        InitializeBlackBoard();
        //MoveIfInRange
        Sequence moveIfInRange = new Sequence(this);
        m_root = moveIfInRange;
        IsWithinRange withingRangeNode = new IsWithinRange(this, "SelfPosition", "TargetPostion");
        MoveTowards moveTowardsNode = new MoveTowards(this, "ownerTransform","TargetPostion");

        moveIfInRange.AddChildNode(withingRangeNode);
        moveIfInRange.AddChildNode(moveTowardsNode);
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
