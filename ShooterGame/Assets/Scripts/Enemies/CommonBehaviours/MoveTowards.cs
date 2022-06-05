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
        Transform ownerTransform = (Transform)GetData(key_ownerGameObject);
        Vector3 targetPostion = (Vector3)GetData(key_targetPosition);

        if (ownerTransform == null || targetPostion==null) 
            return NodeState.Failed;

        Vector3 direction = targetPostion-ownerTransform.position;

        GameManager.instance.GetEventManager().AddEvent(new AIMoveEvent(ownerTransform.gameObject, direction.normalized));

        return NodeState.Sucessful;
    }
}
