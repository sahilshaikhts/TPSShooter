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

    public string key_wPositionToMoveTowards;
    public string key_wMaxDistanceToPlayer;
    public string key_wSpotToShootFrom;


    public MoveInLineOfSight(BehaviourTree aTree, string aiInfo,string aKeyWPositionToMoveTowards, string aKeySpotToShootFrom, string aKeyMaxDistanceToPlayer) : base(aTree) 
    {
        m_aiInfo = (AIInfo)GetData(aiInfo);

        m_ownerCharacter = m_aiInfo.GetOwnerCharacter();
        m_pathFinder = m_aiInfo.GetPathFinder();

        m_weaponLayerMask = m_aiInfo.GetWeaponLayerMask();

        key_wPositionToMoveTowards = aKeyWPositionToMoveTowards;
        key_wMaxDistanceToPlayer= aKeyMaxDistanceToPlayer;
        key_wSpotToShootFrom = aKeySpotToShootFrom;
    }

    public override NodeState Execute()
    {
        Character targetCharacter = m_aiInfo.GetTargetCharacter();
        Vector3? shootableSpotPosition=(Vector3?)GetData(key_wSpotToShootFrom);

        //If this node is being ran for the first time check if can shoot and if not look for spots to shoot from and move towards it.
        if (shootableSpotPosition == null)
        {
            if (CheckIfCanShootTarget(m_ownerCharacter.GetPositionOfHead(), targetCharacter) == false)
            {
                shootableSpotPosition = FindASpotToShootFrom(3);
                //If couldn't find any close spot to shoot from return failure.
                if (shootableSpotPosition.HasValue == false)
                {
                    return NodeState.Failed;
                }
            }
            else
            {
                return NodeState.Sucessful;
            }
        }

        //Move towards spot where the character can shoot from.
        float distanceFromSpot = Vector3.SqrMagnitude(m_ownerCharacter.GetPosition() - (Vector3)shootableSpotPosition);
       
        if (distanceFromSpot > 0.5f)
        {
            m_tree.SetData(key_wPositionToMoveTowards, shootableSpotPosition);
        }
        else
            shootableSpotPosition = null;

        m_tree.SetData(key_wSpotToShootFrom,(Vector3?)shootableSpotPosition);
        
        return NodeState.Sucessful;
    }


    private bool CheckIfCanShootTarget(Vector3 aOriginPosition,Character aTargetCharacter)
    {
        Vector3 direction= aTargetCharacter.GetPositionOfHead()-aOriginPosition;

        Ray ray=new Ray(aOriginPosition, direction.normalized);
        RaycastHit hit;
        
        Debug.DrawRay(ray.origin,ray.direction*100,Color.red,.5f);
        
        if (Physics.SphereCast(ray,1, out hit, m_weaponLayerMask))
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
        List<Vector3> spotsToCheck= m_pathFinder.GetWalkableNeighbourCellsPosition(m_ownerCharacter.GetPosition()+m_ownerCharacter.GetForwardDirection()* (aRadiusOfGridCellsToCheck/2), aRadiusOfGridCellsToCheck); //Get list of neighbouring cells to try and shoot from.
        
        List<Vector3> spotsToShootFrom=new List<Vector3>();

        Character targetCharacter = m_aiInfo.GetTargetCharacter();

        float characterHeight = m_ownerCharacter.GetPositionOfHead().y;

        //Check from each neighbouing cell if this character can shoot target and add that cell to a list.
        foreach (Vector3 spot in spotsToCheck)
        {
            //First check if the spot is within MaxDistacneToPlayer
            if (Vector3.SqrMagnitude(targetCharacter.GetPosition() - spot) < (float)GetData(key_wMaxDistanceToPlayer))
            {
                //Check if we can shoot from the spot(raycast)
                if (CheckIfCanShootTarget(new Vector3(spot.x, characterHeight, spot.z), targetCharacter))
                    spotsToShootFrom.Add(spot);
            }
        }
        
        //From the shootable list of cells find the clossest one.
        Vector3? nearestSpot=null;
        float nearestSpotDist = float.MaxValue;
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
