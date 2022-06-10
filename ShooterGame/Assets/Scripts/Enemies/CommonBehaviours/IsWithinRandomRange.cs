using Behaviourtree;
using UnityEngine;

public class IsWithinRandomRange : Node
{

    public IsWithinRandomRange(BehaviourTree aTree, string aOwnerTargetPostion, string aKeyTargetPostion, float aRangeMin,float aRangeMax) : base(aTree)
    {
        key_ownerPosition = aOwnerTargetPostion;
        key_targetPosition = aKeyTargetPostion; 
        rangeMax = aRangeMax;
        rangeMin = aRangeMin;
    }

    public string key_ownerPosition;
    public string key_targetPosition;
    public float rangeMin,rangeMax;
    public float randomDistance;
    public override NodeState Execute()
    {
        //If node is being executed for the first time,set a new random distance
        if(m_state!=NodeState.Running)
        {
            randomDistance=Random.Range(rangeMin,rangeMax);
        }
        if (m_tree.GetData(key_targetPosition) != null && m_tree.GetData(key_ownerPosition) != null)
        {
            Vector3 position = (Vector3)m_tree.GetData(key_ownerPosition);
            Vector3 targetPosition = (Vector3)m_tree.GetData(key_targetPosition);

            if (Vector3.SqrMagnitude(position - targetPosition) < randomDistance * randomDistance)
                return NodeState.Sucessful;
        }
        return NodeState.Running;
    }
}
