using Behaviourtree;
using EventSystem;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;

public class MoveTowards : Node
{
    Character m_ownerCharacter;
    PathFinder pathFinder;

    public string key_ownerCharacter;
    public string key_targetCharacter;

    public MoveTowards(BehaviourTree aTree, string aKeyOwnerCharacter, string aKeyTargetCharacter) : base(aTree) 
    {
        key_ownerCharacter = aKeyOwnerCharacter;
        key_targetCharacter = aKeyTargetCharacter;
        m_ownerCharacter = (Character)GetData(key_ownerCharacter);
        pathFinder = new PathFinder(GameManager.instance.GetGridForPathFinding());
    }

    public override NodeState Execute()
    {
        Vector3 ownerPosition = m_ownerCharacter.GetPosition();
        Vector3 targetPostion = ((Character)GetData(key_targetCharacter)).GetPosition();
        
        Vector3 direction = pathFinder.GetMoveDirection(ownerPosition, targetPostion);
        
        GameManager.instance.GetEventManager().AddEvent(new AIMoveEvent(m_ownerCharacter, direction.normalized));

        return NodeState.Sucessful;
    }
}
