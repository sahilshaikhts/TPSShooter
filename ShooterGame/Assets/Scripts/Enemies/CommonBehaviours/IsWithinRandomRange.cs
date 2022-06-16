using Behaviourtree;
using ShootingGame;
using UnityEngine;

public class IsWithinRandomRange : Node
{

    public IsWithinRandomRange(BehaviourTree aTree, string aKeyOwnerCharacter, string aKeyTargetCharacter, float aRangeMin, float aRangeMax) : base(aTree)
    {
        key_owner = aKeyOwnerCharacter;
        key_target = aKeyTargetCharacter;

        rangeMax = aRangeMax;
        rangeMin = aRangeMin;
    }

    public string key_owner;
    public string key_target;
    public float rangeMin, rangeMax;
    public float randomDistance;

    public override NodeState Execute()
    {
        if (m_tree.GetData(key_target) != null && m_tree.GetData(key_owner) != null)
        {
            Vector3 position = ((Character)m_tree.GetData(key_owner)).GetPosition();
            Vector3 targetPosition = ((Character)m_tree.GetData(key_target)).GetPosition();

            //If node is being executed for the first time,set a new random distance
            if (m_state != NodeState.Running)
            {
                //Adjust the max range if unit is closer to target than max.
                rangeMax = Mathf.Min(rangeMax, Vector3.Distance(position, targetPosition));

                randomDistance = Random.Range(rangeMin, rangeMax);
            }

            if (Vector3.SqrMagnitude(position - targetPosition) < randomDistance * randomDistance)
                return m_state;
        }
        return m_state;
    }
}
