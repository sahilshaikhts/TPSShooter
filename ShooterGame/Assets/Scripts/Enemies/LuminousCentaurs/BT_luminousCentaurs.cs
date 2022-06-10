using Behaviourtree;
using UnityEngine;

public class BT_luminousCentaurs : BehaviourTree
{
    [SerializeField] Transform m_playerTransform;

    public void Start()
    {
        InitializeBlackBoard();
        Sequence root=new Sequence(this);
        m_root = root;

        //MoveInRangeToShoot
        Selector moveInRangeToShoot = new Selector(this);

        IsWithinRandomRange withinRangeNode = new IsWithinRandomRange(this, "SelfPosition", "TargetPostion",40,100);
        MoveTowards moveTowardsNode = new MoveTowards(this, "ownerTransform","TargetPostion");

        //OnceInRangeShoot
        MoveInLineOfSight
        ShootTarget shootTarget;
        
        m_root.AddChildNode(moveInRangeToShoot);
        moveInRangeToShoot.AddChildNode(moveTowardsNode);
        moveInRangeToShoot.AddChildNode(withinRangeNode);
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
