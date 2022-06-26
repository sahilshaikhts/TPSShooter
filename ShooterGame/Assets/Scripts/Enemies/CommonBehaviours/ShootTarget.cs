using Behaviourtree;
using EventSystem;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;

public class ShootTarget : Node
{
    Character m_ownerCharacter;
    PathFinder pathFinder;

    public string key_ownerCharacter;
    public string key_targetCharacter;
    public string key_bCanShoot;

    public ShootTarget(BehaviourTree aTree, string aKeyOwnerCharacter, string aKeyTargetCharacter, string aKeybCanShoot) : base(aTree)
    {
        key_ownerCharacter = aKeyOwnerCharacter;
        key_targetCharacter = aKeyTargetCharacter;
        m_ownerCharacter = ((Character)m_tree.GetData(key_ownerCharacter));
        key_bCanShoot=aKeybCanShoot;
        pathFinder = new PathFinder(GameManager.instance.GetGridForPathFinding());
    }

    public override NodeState Execute()
    {
        Vector3 ownerPosition = m_ownerCharacter.GetPositionOfHead();
        Vector3 targetPostion = ((Character)GetData(key_targetCharacter)).GetPositionOfHead();
        bool canShoot= (bool)GetData(key_bCanShoot);
        
        if(canShoot==false)
        {
            return NodeState.Failed;
        }

        Vector3 direction = targetPostion - ownerPosition;

        GameManager.instance.GetEventManager().AddEvent(new AIShootTargetEvent(m_ownerCharacter, direction.normalized));

        return NodeState.Sucessful;
    }
}
