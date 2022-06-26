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
    public string key_targetWPosition;

    //TODO: What if this behaviour looked up the cell it needs to move towards set by other behaviour and get executed at the end?s?
    public MoveTowards(BehaviourTree aTree, string aKeyOwnerCharacter,string aKeyPathFinder, string aKeyTargetWPosition) : base(aTree) 
    {
        key_ownerCharacter = aKeyOwnerCharacter;
        key_targetWPosition = aKeyTargetWPosition;
        m_ownerCharacter = (Character)GetData(key_ownerCharacter);
        pathFinder = (PathFinder)GetData(aKeyPathFinder);
    }

    public override NodeState Execute()
    {
        Vector3 ownerPosition = m_ownerCharacter.GetPosition();
        Vector3? targetPosition= ((Vector3?)GetData(key_targetWPosition));

        if (targetPosition == null)
        {
            Debug.Log(null);
            return NodeState.Failed;
        
        }else
            Debug.Log(targetPosition);
        Vector3 direction = pathFinder.GetMoveDirection(ownerPosition, (Vector3)targetPosition);
        
        GameManager.instance.GetEventManager().AddEvent(new AIMoveEvent(m_ownerCharacter, direction.normalized));
        
        //Reset
        m_tree.SetData("key_CellToMoveTowards", null);

        return NodeState.Sucessful;
    }
}
