using Behaviourtree;
using EventSystem;
using Sahil.AStar;
using ShootingGame;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : Node
{
    AIInfo m_aiInfo;
    Character m_ownerCharacter;
    PathFinder m_pathFinder;
    public string key_wPositionToMoveTowards;

    Vector3 directionToMove;
    int lastDirection = 0;
    public Dodge(BehaviourTree aTree, string aiInfo, string aKeyWPositionToMoveTowards) : base(aTree)
    {
        m_aiInfo = (AIInfo)GetData(aiInfo);
        m_pathFinder = m_aiInfo.GetPathFinder();
        m_ownerCharacter = m_aiInfo.GetOwnerCharacter();

        key_wPositionToMoveTowards = aKeyWPositionToMoveTowards;
    }

    public override NodeState Execute()
    {
        Character targetCharacter = m_aiInfo.GetTargetCharacter();

        Vector3 directionFromCamera = (m_ownerCharacter.GetPosition()- GameManager.instance.GetCameraManager().GetCamera("PlayerFollowCamera").gameObject.transform.position).normalized;
        //Todo: change this
        Vector3 camerasRight = GameManager.instance.GetCameraManager().GetCamera("PlayerFollowCamera").gameObject.transform.right;

        float xDirection = -1*Vector3.Dot(camerasRight, directionFromCamera);

        if (xDirection > 0)
            xDirection = 1;
        else
            xDirection = -1;

        directionToMove = m_ownerCharacter.transform.right * xDirection;

        List<Vector3> moveableSpot = m_pathFinder.GetWalkablePathInDirection(m_ownerCharacter.GetPosition(), directionToMove, 1);

        //check if unit can move in the calculated direction
        if (moveableSpot==null)
        {
            //if can't walk on the desired direction try the oppoisite,if still can't return failed,else set the direction to the opposite one.
            moveableSpot = m_pathFinder.GetWalkablePathInDirection(m_ownerCharacter.GetPosition(), -directionToMove, 1);
            if (moveableSpot==null)
            {
                return NodeState.Failed;
            }
        }

        m_tree.SetData(key_wPositionToMoveTowards, (Vector3?)moveableSpot[0]);

        return NodeState.Sucessful;
    }
}
