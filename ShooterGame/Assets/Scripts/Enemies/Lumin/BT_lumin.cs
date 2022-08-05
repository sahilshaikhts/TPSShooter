using Behaviourtree;
using Sahil;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;

public class BT_lumin : BehaviourTree
{
    AIInfo m_aiInfo;
    [SerializeField] private Character m_self;
    [SerializeField] private Character m_player;
    [SerializeField] private LayerMask m_layerMask;

    private PathFinder m_pathFinder;

    string key_aiInfo="AIInfo";

    public void Start()
    {
        m_pathFinder = new PathFinder(GameManager.instance.GetGridForPathFinding());

        m_aiInfo = new AIInfo(m_self, m_player, m_pathFinder, m_layerMask);

        InitializeBlackBoard();
        Selector root = new Selector(this);
        m_root = root;

        //**** TODO: Add check for if about to or being shot at ,defend or try to shoot.
        Sequence movementTree = new Sequence(this);
        Selector evaluateWhereToMove = new Selector(this);
        Sequence moveStrategically = new Sequence(this);
        ExecuteAll movingTowardsPlayer=new ExecuteAll(this);

        WithinDistance withinRangeNode = new WithinDistance(this, "AIInfo", "MaxDistanceFromPlayer");
        SetValue<Vector3> setMovePositionToTarget = new SetValue<Vector3>(this, "wPositionToMoveTowards", "TargetPosition");
        SetValue<Vector3?> resetSpotToShootFrom = new SetValue<Vector3?>(this, "SpotToShootFrom", (Vector3?)null);

        Sequence aggresive = new Sequence(this);
        //IfInLineOfSight
        MoveInLineOfSight moveInLineOfSight = new MoveInLineOfSight(this, "AIInfo", "wPositionToMoveTowards", "SpotToShootFrom", "MaxDistanceFromPlayer");

        ShootTarget shootTarget = new ShootTarget(this, "AIInfo");


        //MoveTowards player
        MoveTowards moveTowardsNode = new MoveTowards(this, "AIInfo", "wPositionToMoveTowards");

        //Root
        m_root.AddChildNode(movementTree);
        {
            movementTree.AddChildNode(evaluateWhereToMove);
            {
                evaluateWhereToMove.AddChildNode(moveStrategically);
                {
                    moveStrategically.AddChildNode(withinRangeNode);
                    moveStrategically.AddChildNode(aggresive);
                    aggresive.AddChildNode(moveInLineOfSight);
                }
                evaluateWhereToMove.AddChildNode(movingTowardsPlayer);
                {
                    movingTowardsPlayer.AddChildNode(setMovePositionToTarget);
                    movingTowardsPlayer.AddChildNode(resetSpotToShootFrom);
                }
            }
            movementTree.AddChildNode(moveTowardsNode);
        }
    }

    void InitializeBlackBoard()
    {
        AddKey("AIInfo");
        AddKey("wPositionToMoveTowards");
        AddKey("ownerHeadPosition");
        AddKey("TargetPosition");
        AddKey("SpotToShootFrom");
        AddKey("allowedToShoot");

        SetData("AIInfo", (AIInfo)m_aiInfo);
        SetData("wPositionToMoveTowards", (Vector3?)null);
        SetData("MaxDistanceFromPlayer", (float)20);
    }
    private void Update()
    {
        //Update blackboard data
        SetData("TargetPosition", (Vector3)m_aiInfo.GetTargetCharacter().GetPosition());

        base.UpdateNodes();
        m_aiInfo.ResetCalculatedData();
    }
}

/// <summary>
/// AIInfo class contains data for the nodes to use.
/// Warning!!: Remember to call the ResetCalculatedData() after executing/Updating the behaviour tree node.
/// </summary>
class AIInfo
{
    private Character m_owner;
    private Character m_target;
    private PathFinder m_pathFinder;

    private Cell m_cellToMoveTowards;
    private LayerMask m_weaponLayerMask;
    private float m_distanceToTarget =-1;
    private float m_sqrDistanceToTarget =-1;

    public bool allowedToShoot;// Meant to be changed by AI logic,such as false during cooldown, waiting for unit's turn.
    public bool canShoot;//Meant to be changed based on ammo,Line of sight etc.

    public AIInfo(Character aOwner, Character aTarget, PathFinder pathFinder, LayerMask aWeaponLayerMask)
    {
        m_owner = aOwner;
        m_target = aTarget;
        m_pathFinder = pathFinder;
        m_weaponLayerMask = aWeaponLayerMask;
    }

    public void ResetCalculatedData()
    {
        m_distanceToTarget = -1;
        m_sqrDistanceToTarget = -1;
    }

    //Getters
    public PathFinder GetPathFinder() { return m_pathFinder; }
    public Cell GetCellToMoveTowards() { return m_cellToMoveTowards; }

    public float GetDistanceToTarget()
    {
        //if distance never set in this frame
        if (m_distanceToTarget == -1)
        {
            if (m_owner != null && m_target != null)
                m_distanceToTarget = Vector3.Distance(m_owner.GetPosition(), m_target.GetPosition());
            else
                Debug.LogError("Null Error");
        }
        return m_distanceToTarget;
    }
    public float GetSquareDistanceToTarget()
    {
        //if sqrDistance never set in this frame
        if (m_sqrDistanceToTarget == -1)
        {
            if (m_owner != null && m_target != null)
                m_sqrDistanceToTarget = Vector3.SqrMagnitude(m_owner.GetPosition()- m_target.GetPosition());
            else
                Debug.LogError("Null Error");
        }
        return m_sqrDistanceToTarget;
    }

    public Character GetOwnerCharacter() { return m_owner; }
    public Character GetTargetCharacter() { return m_target; }

    public LayerMask GetWeaponLayerMask() { return m_weaponLayerMask; }
    //Setters
    public void SetCellToMoveTowards(Cell aCell) { m_cellToMoveTowards=aCell; }
}
 
