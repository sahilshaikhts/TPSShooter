using Behaviourtree;
using EventSystem;
using UnityEngine;

public class MoveTowards : Node
{
    public MoveTowards(BehaviourTree aTree, string aKeyOwnerGameObject, string aKeyTargetPostion) : base(aTree) 
    {
        key_ownerGameObject = aKeyOwnerGameObject; 
        key_targetPosition = aKeyTargetPostion; 
    }

    public string key_ownerGameObject;
    public string key_targetPosition;

    public override NodeState Execute()
    {
        GameObject ownerObj = (GameObject)GetData(key_ownerGameObject);
        Vector3 targetPostion = (Vector3)GetData(key_targetPosition);

        if (ownerObj==null || targetPostion==null) 
            return NodeState.Failed;

        Vector3 direction = ownerObj.transform.position - targetPostion;

        GameManager.GetEventManager().AddEvent(new MoveEvent(ownerObj, direction.normalized));

        return NodeState.Running;
    }
}
