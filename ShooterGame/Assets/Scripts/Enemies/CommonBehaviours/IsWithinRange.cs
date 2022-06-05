using Behaviourtree;
using UnityEngine;

public class IsWithinRange : Node
{
    public IsWithinRange(BehaviourTree aTree, string aOwnerTargetPostion, string aKeyTargetPostion) : base(aTree) { key_ownerPosition = aOwnerTargetPostion; key_targetPosition = aKeyTargetPostion; }

    public IsWithinRange(BehaviourTree aTree, string aOwnerTargetPostion, string aKeyTargetPostion, float aDistance) : base(aTree)
    {
        key_ownerPosition = aOwnerTargetPostion;
        key_targetPosition = aKeyTargetPostion; 
        distance = aDistance;
    }

    public string key_ownerPosition;
    public string key_targetPosition;
    public float distance = 10;

    public override NodeState Execute()
    {
        if (m_tree.GetData(key_targetPosition) != null && m_tree.GetData(key_ownerPosition) != null)
        {
            Vector3 position = (Vector3)m_tree.GetData(key_ownerPosition);
            Vector3 targetPosition = (Vector3)m_tree.GetData(key_targetPosition);

            if (Vector3.Distance(position, targetPosition) < distance)
                return NodeState.Sucessful;
        }
        return NodeState.Running;
    }
}
