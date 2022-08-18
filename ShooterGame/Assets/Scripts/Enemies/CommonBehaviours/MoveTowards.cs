using Behaviourtree;
using EventSystem;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;

public class MoveTowards : Node
{
    public string key_aiInfo;
    public string key_targetWPosition;

    public MoveTowards(BehaviourTree aTree, string aKeyAIInfo,string aKeyTargetWPosition) : base(aTree) 
    {
        key_aiInfo = aKeyAIInfo;
        key_targetWPosition = aKeyTargetWPosition;
    }

    public override NodeState Execute()
    {
        AIInfo aIInfo = (AIInfo)GetData(key_aiInfo);
        Vector3 ownerPosition = aIInfo.GetOwnerCharacter().GetPosition();
        Vector3? targetPosition= ((Vector3?)GetData(key_targetWPosition));

        if (targetPosition == null)
            return NodeState.Failed;

        Vector3 direction = aIInfo.GetPathFinder().GetMoveDirection(ownerPosition, (Vector3)targetPosition);
        
        GameManager.instance.GetEventManager().AddEvent(new AIMoveEvent(aIInfo.GetOwnerCharacter(), direction.normalized));
        
        //Reset
        m_tree.SetData(key_targetWPosition, null);

        return NodeState.Sucessful;
    }
}
