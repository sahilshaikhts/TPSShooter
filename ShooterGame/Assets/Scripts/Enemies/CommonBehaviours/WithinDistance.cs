using Behaviourtree;
using Sahil.AStar;
using ShootingGame;
using UnityEngine;

public class WithinDistance : Node
{
    private string key_AIInfo;
    private string key_DistanceToCheck;
    private float m_DistanceToCheck;

    public WithinDistance(BehaviourTree aTree, string aKeyAIInfo,float aDistance) : base(aTree)
    {
        key_AIInfo = aKeyAIInfo;
        m_DistanceToCheck = aDistance;
    }

    public WithinDistance(BehaviourTree aTree, string aKeyAIInfo, string aKeyDistance) : base(aTree)
    {
        key_AIInfo = aKeyAIInfo;
        key_DistanceToCheck =aKeyDistance ;
    }

    public override NodeState Execute()
    {
        float currentSqrDistance= ((AIInfo)m_tree.GetData(key_AIInfo)).GetSquareDistanceToTarget();

        if(key_DistanceToCheck!=null)
        {
            m_DistanceToCheck = (float)m_tree.GetData(key_DistanceToCheck);
        }

        if (currentSqrDistance < (m_DistanceToCheck * m_DistanceToCheck))
        {
            return NodeState.Sucessful;
        }
        else
        {
            return NodeState.Failed;
        }
    }
}
