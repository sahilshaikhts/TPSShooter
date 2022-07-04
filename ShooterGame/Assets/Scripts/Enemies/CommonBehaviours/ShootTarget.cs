using Behaviourtree;
using EventSystem;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;

public class ShootTarget : Node
{
    public string key_aiInfo;

    public ShootTarget(BehaviourTree aTree, string aKeyAIInfo) : base(aTree)
    {
        key_aiInfo = aKeyAIInfo;
    }

    public override NodeState Execute()
    {
        AIInfo aIInfo=(AIInfo)GetData(key_aiInfo);
        Vector3 ownerPosition = aIInfo.GetOwnerCharacter().GetPositionOfHead();
        Vector3 targetPostion = aIInfo.GetTargetCharacter().GetPositionOfHead();
        
        if(aIInfo.canShoot == false)
        {
            return NodeState.Failed;
        }

        Vector3 direction = targetPostion - ownerPosition;

        GameManager.instance.GetEventManager().AddEvent(new AIShootTargetEvent(aIInfo.GetOwnerCharacter(), direction.normalized));

        return NodeState.Sucessful;
    }
}
