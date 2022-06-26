using Behaviourtree;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;

public class IsWithinRandomRange : Node
{
    private string key_owner;
    private string key_target;
    private string key_wPositionToMoveTowards;

    private float rangeMin, rangeMax;
    private float randomDistance=20;

    public IsWithinRandomRange(BehaviourTree aTree, string aKeyOwnerCharacter, string aKeyTargetCharacter, string aKeywPositionToMoveTowards, float aRangeMin, float aRangeMax) : base(aTree)
    {
        key_owner = aKeyOwnerCharacter;
        key_target = aKeyTargetCharacter;

        rangeMax = aRangeMax;
        rangeMin = aRangeMin;

        key_wPositionToMoveTowards = aKeywPositionToMoveTowards;
    }

    public override NodeState Execute()
    {
        Vector3 position = ((Character)m_tree.GetData(key_owner)).GetPosition();
        Vector3? targetPosition = ((Character)m_tree.GetData(key_target)).GetPosition();

        float distance = Vector3.SqrMagnitude(position - (Vector3)targetPosition);

        if (distance < (randomDistance * randomDistance))
        {
            Debug.Log("InRange");
            return NodeState.Sucessful;
        }
        else
        {
            m_tree.SetData(key_wPositionToMoveTowards, targetPosition);
            Debug.LogWarning("set " + distance);
            return NodeState.Failed;
        }
    }
}
