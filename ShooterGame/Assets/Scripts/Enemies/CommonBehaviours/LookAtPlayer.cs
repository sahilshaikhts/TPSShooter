using Behaviourtree;
using EventSystem;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;

public class LookAtPlayer : Node
{
    public string key_aiInfo;

    public LookAtPlayer(BehaviourTree aTree, string aKeyAIInfo) : base(aTree) 
    {
        key_aiInfo = aKeyAIInfo;
    }

    public override NodeState Execute()
    {
        AIInfo aIInfo = (AIInfo)GetData(key_aiInfo);
        Vector3 direction = (aIInfo.GetTargetCharacter().GetPosition()-aIInfo.GetOwnerCharacter().GetPosition()).normalized;

        GameManager.instance.GetEventManager().AddEvent(new AILookTowardsEvent(aIInfo.GetOwnerCharacter(), direction));

        return NodeState.Sucessful;
    }
}
