using Behaviourtree;
using EventSystem;
using Sahil.AStar;
using UnityEngine;

public class ShootTarget : Node
{
    public string key_ownerGameObject;
    public string key_targetPosition;
    PathFinder pathFinder;

    public ShootTarget(BehaviourTree aTree, string aKeyOwnerGameObject, string aKeyTargetPostion) : base(aTree) 
    {
        key_ownerGameObject = aKeyOwnerGameObject; 
        key_targetPosition = aKeyTargetPostion; 
        pathFinder = new PathFinder(GameManager.instance.GetGridForPathFinding());
    }

    public override NodeState Execute()
    {
        Transform ownerTransform = (Transform)GetData(key_ownerGameObject);
        Vector3 targetPostion = (Vector3)GetData(key_targetPosition);
        
        Cell_AStar nextCell= pathFinder.CalculatePath(ownerTransform.position, targetPostion)[1];

        if (ownerTransform == null || targetPostion==null) 
            return NodeState.Failed;

        Vector3 direction = nextCell.GetWorldPosition()- ownerTransform.position;
        
        GameManager.instance.GetEventManager().AddEvent(new AIMoveEvent(ownerTransform.gameObject, direction.normalized));

        return NodeState.Sucessful;
    }
}
