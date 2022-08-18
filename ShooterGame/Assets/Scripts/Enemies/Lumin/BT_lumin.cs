using Behaviourtree;
using Sahil;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;
using EventSystem;
public class BT_lumin : BehaviourTree
{
    AIInfo m_aiInfo;
    [SerializeField] private Character m_self;
    [SerializeField] private Character m_player;
    [SerializeField] private LayerMask m_layerMask;

    private PathFinder m_pathFinder;

    string key_aiInfo = "AIInfo";

    public void Start()
    {
        GameManager.instance.GetEventManager().SubscribeToEvent(PlayerWeaponActionEvent.EventType(), HandlePlayerAboutToShoot);
        m_pathFinder = new PathFinder(GameManager.instance.GetGridForPathFinding());

        m_aiInfo = new AIInfo(m_self, m_player, m_pathFinder, 4,15, m_layerMask);

        InitializeBlackBoard();
        Selector root = new Selector(this);
        m_root = root;

        //**** TODO: Add check for if about to or being shot at ,defend or try to shoot.
        Sequence movementTree = new Sequence(this);
        Selector evaluateWhereToMove = new Selector(this);
        Selector moveStrategically = new Selector(this);
        ExecuteAll movingTowardsPlayer = new ExecuteAll(this);

        WithinDistance withinRangeNode = new WithinDistance(this, "AIInfo", "MaxDistanceFromPlayer");
        SetValue<Vector3> setMovePositionToTarget = new SetValue<Vector3>(this, "wPositionToMoveTowards", "TargetPosition");
        SetValue<Vector3?> resetSpotToShootFrom = new SetValue<Vector3?>(this, "SpotToShootFrom", (Vector3?)null);

        Sequence defensive = new Sequence(this);

        BoolCondition ifNotInDefensiveCooldown = new BoolCondition(this, "bDefenseCooldown", false);

        Selector evaluateIfInDanger = new Selector(this);
        BoolCondition isAlreadyEngagedInAction = new BoolCondition(this, "bEngagedInAction");

        Vector2[] rangeToSenseDanger = { new Vector2(1, 5), new Vector2(3, 5) };

        SelectFromRange senseDangerBasedOnIntelligence = new SelectFromRange(this, rangeToSenseDanger, "fIntelligence");

        BoolCondition checkIfGotShot = new BoolCondition(this, "bGotShot");
        BoolCondition checkIfBeingAimedAt = new BoolCondition(this, "bBeingAimedAt");

        Sequence evaluateDefensiveMove = new Sequence(this);
        SelectFromRange decideDefenseBasedOnIntelligence = new SelectFromRange(this, rangeToSenseDanger, "fIntelligence");
        SetValue<int> selectDodge = new SetValue<int>(this, "sDefenseMove", 0);
        //SetValue<string> selectCover = new SetValue<string>(this, "sDefenseMove", "cover");

        SetValue<bool> setEngagedInAction = new SetValue<bool>(this, "bEngagedInAction", true);

        Selector executeDefenseMove = new Selector(this);
        Sequence dodgeToDefend = new Sequence(this);

        Condition isDodgeSelected = new Condition(this, () => ((int)GetData("sDefenseMove") == 0));
        Dodge dodge = new Dodge(this, "AIInfo", "wPositionToMoveTowards");

        Sequence aggresive = new Sequence(this);
        //IfInLineOfSight
        MoveInLineOfSight moveInLineOfSight = new MoveInLineOfSight(this, "AIInfo", "wPositionToMoveTowards", "SpotToShootFrom", "MaxDistanceFromPlayer");

        ShootTarget shootTarget = new ShootTarget(this, "AIInfo");

        //MoveTowards player
        MoveTowards moveTowardsNode = new MoveTowards(this, "AIInfo", "wPositionToMoveTowards");
        LookAtPlayer lookAtPlayer = new LookAtPlayer(this, "AIInfo");

        //Root
        m_root.AddChildNode(movementTree);
        {
            movementTree.AddChildNode(evaluateWhereToMove);
            {
                evaluateWhereToMove.AddChildNode(moveStrategically);
                {
                    moveStrategically.AddChildNode(defensive);
                    {
                        defensive.AddChildNode(ifNotInDefensiveCooldown);
                        defensive.AddChildNode(evaluateIfInDanger);
                        {
                            //evaluateIfInDanger.AddChildNode(isAlreadyEngagedInAction);
                            evaluateIfInDanger.AddChildNode(evaluateDefensiveMove);
                            {
                                evaluateDefensiveMove.AddChildNode(senseDangerBasedOnIntelligence);
                                senseDangerBasedOnIntelligence.AddChildNode(checkIfGotShot);
                                senseDangerBasedOnIntelligence.AddChildNode(checkIfBeingAimedAt);

                                evaluateDefensiveMove.AddChildNode(decideDefenseBasedOnIntelligence);
                                decideDefenseBasedOnIntelligence.AddChildNode(selectDodge);
                                // decideDefenseBasedOnIntelligence.AddChildNode(selectCover);

                                evaluateDefensiveMove.AddChildNode(setEngagedInAction);
                            }
                        }
                        defensive.AddChildNode(executeDefenseMove);
                        {
                            executeDefenseMove.AddChildNode(dodgeToDefend);
                            dodgeToDefend.AddChildNode(isDodgeSelected);
                            dodgeToDefend.AddChildNode(dodge);
                            dodgeToDefend.AddChildNode(lookAtPlayer);
                        }
                    }
                    moveStrategically.AddChildNode(aggresive);
                    aggresive.AddChildNode(withinRangeNode);
                    aggresive.AddChildNode(moveInLineOfSight);
                }
                evaluateWhereToMove.AddChildNode(withinRangeNode);
                evaluateWhereToMove.AddChildNode(movingTowardsPlayer);
                {
                    movingTowardsPlayer.AddChildNode(setMovePositionToTarget);
                    movingTowardsPlayer.AddChildNode(lookAtPlayer);
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

        AddKey("sDefenseMove");
        AddKey("fIntelligence");

        AddKey("bAllowedToShoot");
        AddKey("bDefenseCooldown");
        AddKey("bAttackCooldown");
        AddKey("bEngagedInAction");
        AddKey("bGotShot");
        AddKey("bBeingAimedAt");


        SetData("AIInfo", (AIInfo)m_aiInfo);
        SetData("wPositionToMoveTowards", (Vector3?)null);
        SetData("MaxDistanceFromPlayer", (float)20);

        SetData("fIntelligence", (float)m_aiInfo.GetIntelligence()); ;

        SetData("sDefenseMove", (int)-1);//Options:0=dodge,1=cover;

        SetData("bDefenseCooldown", (bool)false);
        SetData("bAttackCooldown", (bool)false);
        SetData("bEngagedInAction", (bool)false);
        SetData("bAllowedToShoot", (bool)false);
        SetData("bGotShot", (bool)false);
        SetData("bBeingAimedAt", (bool)false);
    }
    private void Update()
    {
        //Update blackboard data
        SetData("TargetPosition", (Vector3)m_aiInfo.GetTargetCharacter().GetPosition());

        base.UpdateNodes();
        m_aiInfo.ResetDataFromThisFrame();
    }

    void HandlePlayerAboutToShoot(IEvent aEvent)
    {
        PlayerWeaponActionEvent playerWeaponActionEvent = (PlayerWeaponActionEvent)aEvent;

        if (playerWeaponActionEvent.bWeaponDrawn == false)
        {
            SetData("bBeingAimedAt", (bool)false);
        }
        else
        if (m_aiInfo.GetSquareDistanceToTarget() < m_aiInfo.GetSensorRange()* m_aiInfo.GetSensorRange())
        {
            Camera camera = GameManager.instance.GetCameraManager().GetCamera("PlayerFollowCamera").GetCamera();

            Vector3 playerToSelfDir = m_self.transform.position - camera.transform.position;
            Vector3 cameraDirection = camera.GetComponent<Camera>().transform.forward;

            cameraDirection.y = playerToSelfDir.y = 0;

            float dot = Mathf.Clamp(Vector3.Dot(cameraDirection, playerToSelfDir.normalized), -1, 1);

            float angleBetweenEnemy = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angleBetweenEnemy < 10)
            {
                SetData("bBeingAimedAt", (bool)true);
            }
        }
    }
}

/// <summary>
/// AIInfo class contains data for the nodes to use.
/// Warning!!: Remember to call the ResetDataFromThisFrame() after executing/Updating the behaviour tree node.
/// </summary>
class AIInfo
{
    private Character m_owner, m_target;
    private PathFinder m_pathFinder;

    private Cell m_cellToMoveTowards;
    private LayerMask m_weaponLayerMask;

    private Vector3 m_targetDirection = Vector3.zero;
    private float m_distanceToTarget = -1, m_sqrDistanceToTarget = -1;
    private float m_sensorRange;//Range withing which unit can detect target's actions
    private float m_intelligence;

    public bool bAllowedToShoot;// Meant to be changed by AI logic,such as false during cooldown, waiting for unit's turn.
    public bool bCanShoot;//Meant to be changed based on ammo,Line of sight etc.

    public bool bBeingAimmedAt, bGotShot;

    public AIInfo(Character aOwner, Character aTarget, PathFinder pathFinder, float aIntelligence,float aSensorRange, LayerMask aWeaponLayerMask)
    {
        m_owner = aOwner;
        m_target = aTarget;
        m_pathFinder = pathFinder;
        m_weaponLayerMask = aWeaponLayerMask;
        m_intelligence = aIntelligence;
        m_sensorRange = aSensorRange;
    }

    public void ResetDataFromThisFrame()
    {
        m_targetDirection = Vector3.zero;
        m_distanceToTarget = -1;
        m_sqrDistanceToTarget = -1;
        bBeingAimmedAt = false;
        bGotShot = false;
    }

    //Getters
    public PathFinder GetPathFinder() { return m_pathFinder; }
    public Cell GetCellToMoveTowards() { return m_cellToMoveTowards; }

    public Vector3 GetTargetDirection()
    {
        //if m_targetDirection never set in this frame
        if (m_targetDirection == Vector3.zero)
        {
            if (m_owner != null && m_target != null)
                m_targetDirection = (m_target.GetPosition() - m_owner.GetPosition()).normalized;
            else
                Debug.LogError("Null Error");
        }
        return m_targetDirection;
    }
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
    public float GetIntelligence() { return m_intelligence; }
    public float GetSensorRange() { return m_sensorRange; }
    public float GetSquareDistanceToTarget()
    {
        //if sqrDistance never set in this frame
        if (m_sqrDistanceToTarget == -1)
        {
            if (m_owner != null && m_target != null)
                m_sqrDistanceToTarget = Vector3.SqrMagnitude(m_owner.GetPosition() - m_target.GetPosition());
            else
                Debug.LogError("Null Error");
        }
        return m_sqrDistanceToTarget;
    }

    public Character GetOwnerCharacter() { return m_owner; }
    public Character GetTargetCharacter() { return m_target; }

    public LayerMask GetWeaponLayerMask() { return m_weaponLayerMask; }
    //Setters
    public void SetCellToMoveTowards(Cell aCell) { m_cellToMoveTowards = aCell; }
}

