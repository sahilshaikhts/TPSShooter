using Behaviourtree;
using ShootingGame;
using UnityEngine;

public class BT_luminousCentaurs : BehaviourTree
{
    [SerializeField] Character m_self;
    [SerializeField] Character m_player;
    [SerializeField]LayerMask m_layerMask;
    public void Start()
    {
        InitializeBlackBoard();
        Selector root=new Selector(this);
        m_root = root;

        //**** TODO: Add check for if about to or being shot at ,defend or try to shoot.

        //ShootIfInRange
        Sequence shootIfInRange = new Sequence(this);
        
        IsWithinRandomRange withinRangeNode = new IsWithinRandomRange(this, "SelfCharacter", "TargetCharacter", 40,100);

        Sequence shootIfInSight = new Sequence(this);
        //IfInLineOfSight
        MoveInLineOfSight moveInLineOfSight=new MoveInLineOfSight(this, "SelfCharacter", "TargetCharacter", m_layerMask);

        ShootTarget shootTarget=new ShootTarget(this, "SelfCharacter", "TargetCharacter");
        

        //MoveTowards player
        MoveTowards moveTowardsNode = new MoveTowards(this, "SelfCharacter", "TargetCharacter");
        
        
        m_root.AddChildNode(shootIfInSight);
        shootIfInSight.AddChildNode(moveInLineOfSight);
        shootIfInSight.AddChildNode(shootTarget);

        //moveInRangeToShoot.AddChildNode(moveTowardsNode);
        //moveInRangeToShoot.AddChildNode(withinRangeNode);
    }

    void InitializeBlackBoard()
    {
        AddKey("SelfCharacter");
        AddKey("TargetCharacter");
        SetData("SelfCharacter", (Character)m_self);
        SetData("TargetCharacter", (Character)m_player);
    }
    private void Update()
    {
        base.UpdateNodes();
    }
}
