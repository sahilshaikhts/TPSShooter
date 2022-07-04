using Behaviourtree;
using EventSystem;
using Sahil.AStar;
using ShootingGame;
using System.Collections.Generic;
using UnityEngine;

public class MoveInLineOfSight : Node
{
    AIInfo m_aiInfo;
    Character m_ownerCharacter;
    LayerMask m_weaponLayerMask;
    PathFinder m_pathFinder;
    Vector3? m_shootableSpotPosition;
    public string key_wPositionToMoveTowards;


    public MoveInLineOfSight(BehaviourTree aTree, string aiInfo,string aKeyWPositionToMoveTowards) : base(aTree) 
    {
        m_aiInfo = (AIInfo)GetData(aiInfo);

        m_ownerCharacter = m_aiInfo.GetOwnerCharacter();
        m_pathFinder = m_aiInfo.GetPathFinder();

        m_weaponLayerMask = m_aiInfo.GetWeaponLayerMask();

        key_wPositionToMoveTowards = aKeyWPositionToMoveTowards;
    }

    public override NodeState Execute()
    {
        Character targetCharacter = m_aiInfo.GetTargetCharacter();

        //If this node is being ran for the first time check if can shoot and if not look for spots to shoot from and move towards it.
        if (m_shootableSpotPosition == null)
        {
            if (CheckIfCanShootTarget(m_ownerCharacter.GetPositionOfHead(), targetCharacter) == false)
            {
                m_shootableSpotPosition = FindASpotToShootFrom(3);
            }
            else
            {
                m_shootableSpotPosition = null;
            }
        }
        else //If m_shootableSpotPosition already assigned check if it is still valid, if not find different spot.
        {
            if(CheckIfCanShootTarget((Vector3)m_shootableSpotPosition, targetCharacter)==false)
            {
                m_shootableSpotPosition = FindASpotToShootFrom(3);
            }
        }
         
        //If couldn't find any close spot to shoot from return failure.
        if(m_shootableSpotPosition.HasValue==false)
        {
            return NodeState.Failed;
        }

        //Move towards spot where the character can shoot from.
        m_tree.SetData(key_wPositionToMoveTowards, m_shootableSpotPosition);

        return NodeState.Sucessful;
    }


    private bool CheckIfCanShootTarget(Vector3 aOriginPosition,Character aTargetCharacter)
    {
        Vector3 direction= aTargetCharacter.GetPositionOfHead()-aOriginPosition;
        Ray ray=new Ray(aOriginPosition, direction.normalized);
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction*100,Color.red,.5f);
        if (Physics.Raycast(ray, out hit, m_weaponLayerMask))
        {
            //If ray hits target return true.
            if (hit.transform.gameObject.layer == aTargetCharacter.GetCharacterLayer())
            {
                return true;
            }
        }
        return false;
    }

    private Vector3? FindASpotToShootFrom(int aRadiusOfGridCellsToCheck)
    {
        List<Vector3> spotsToCheck= m_pathFinder.GetNeightbouringCellsPosition(m_ownerCharacter.GetPosition(), aRadiusOfGridCellsToCheck); //Get list of neighbouring cells to try and shoot from.
        
        List<Vector3> spotsToShootFrom=new List<Vector3>();

        Character targetCharacter = m_aiInfo.GetTargetCharacter();

        Vector3 characterHeight = m_ownerCharacter.GetPositionOfHead()-m_ownerCharacter.GetPosition();

        //Check from each neighbouing cell if this character can shoot target and add that cell to a list.
        foreach (Vector3 spot in spotsToCheck)
        {
            if (CheckIfCanShootTarget(characterHeight + spot, targetCharacter))
                spotsToShootFrom.Add(spot);
        }
        
        //From the shootable list of cells find the clossest one.
        Vector3? nearestSpot=null;
        float nearestSpotDist = float.MaxValue;
        List<float> nearestSpotss=new List<float>();
        for(int i=0;i<spotsToShootFrom.Count;i++)
        {
            if ((m_ownerCharacter.GetPosition() - spotsToShootFrom[i]).sqrMagnitude < nearestSpotDist)
            {
                nearestSpot = spotsToShootFrom[i];
                nearestSpotDist = (m_ownerCharacter.GetPosition() - spotsToShootFrom[i]).sqrMagnitude;
            }
        }

        return nearestSpot;
    }
}
