using Behaviourtree;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;

public class BT_luminousCentaurs : BehaviourTree
{
    [SerializeField] private Character m_self;
    [SerializeField] private Character m_player;
    [SerializeField] private LayerMask m_layerMask;

    private PathFinder pathFinder;

    public void Start()
    {
        pathFinder = new PathFinder(GameManager.instance.GetGridForPathFinding());


        InitializeBlackBoard();
        Selector root=new Selector(this);
        m_root = root;

        //**** TODO: Add check for if about to or being shot at ,defend or try to shoot.
        Sequence evaluateWhereToMove=new Sequence(this);
        //ShootIfInRange
        Sequence shootIfInRange = new Sequence(this);

        IsWithinRandomRange withinRangeNode = new IsWithinRandomRange(this, "SelfCharacter", "TargetCharacter", "wPositionToMoveTowards", 40,100);

        Invertor moveStrategicallyInvertor=new Invertor(this);
        Selector moveStrategically=new Selector(this);


        Sequence aggresive = new Sequence(this);
        //IfInLineOfSight
        MoveInLineOfSight moveInLineOfSight=new MoveInLineOfSight(this, "SelfCharacter", "TargetCharacter", "wPositionToMoveTowards","PathFinder", "CanShoot", m_layerMask);
            
        ShootTarget shootTarget=new ShootTarget(this, "SelfCharacter", "TargetCharacter", "CanShoot");
        

        //MoveTowards player
        MoveTowards moveTowardsNode = new MoveTowards(this, "SelfCharacter","PathFinder", "wPositionToMoveTowards");
        
        
        m_root.AddChildNode(evaluateWhereToMove);

        evaluateWhereToMove.AddChildNode(withinRangeNode);

        evaluateWhereToMove.AddChildNode(moveStrategicallyInvertor);
            moveStrategicallyInvertor.AddChildNode(moveStrategically);
            moveStrategically.AddChildNode(aggresive);
                    aggresive.AddChildNode(moveInLineOfSight);

        m_root.AddChildNode(moveTowardsNode);

    }

    void InitializeBlackBoard()
    {
        AddKey("PathFinder");
        AddKey("CanShoot");

        AddKey("SelfCharacter");
        AddKey("wPositionToMoveTowards");
        AddKey("TargetCharacter");

        SetData("SelfCharacter", (Character)m_self);
        SetData("PathFinder", (PathFinder)pathFinder);
        SetData("CanShoot", (bool)false);
        SetData("wPositionToMoveTowards", (Vector3?)null);
        SetData("TargetCharacter", (Character)m_player);
    }
    private void Update()
    {
        base.UpdateNodes();
    }
}

/* 
 class AIInfo
{
public:
bCanShoot;
bAllowedToShoot;
bIsReloading

coolDownTimer
Aggresive curve
Defensive curve

}
 
 */