using Behaviourtree;
using EventSystem;
using Sahil.AStar;
using ShootingGame;
using System.Collections.Generic;
using UnityEngine;

public class MoveInLineOfSight : Node
{
    Character m_ownerCharacter;
    PathFinder pathFinder;
    LayerMask m_weaponLayerMask;

    Vector3 m_shootableSpotPosition;

    public string key_ownerCharacter;
    public string key_targetCharacter;


    public MoveInLineOfSight(BehaviourTree aTree, string aKeyOwnerCharacter, string aKeyTargetCharacter, LayerMask aWeaponRayMask) : base(aTree) 
    {
        key_ownerCharacter = aKeyOwnerCharacter;
        key_targetCharacter = aKeyTargetCharacter;
        m_ownerCharacter = ((Character)m_tree.GetData(key_ownerCharacter));
        pathFinder = new PathFinder(GameManager.instance.GetGridForPathFinding());
        m_weaponLayerMask = aWeaponRayMask;
    }

    public override NodeState Execute()
    {
        Vector3 ownerPosition = m_ownerCharacter.GetPosition();
        Character targetCharacter = (Character)GetData(key_targetCharacter);

        //If this node is being ran for the first time check if can shoot and if not look for spots to shoot from and move towards it.
        if(true)
        {
            if (CheckIfCanShootTarget(m_ownerCharacter.GetPositionOfHead(), targetCharacter) == false)
            {
                m_shootableSpotPosition = FindASpotToShootFrom(3);
            }
            else
            {
                m_shootableSpotPosition = Vector3.negativeInfinity;
                return NodeState.Sucessful;
            }
        }
        else //If m_shootableSpotPosition already assigned check if it is still valid, if not find different spot.
        {
            if(CheckIfCanShootTarget(m_shootableSpotPosition, targetCharacter)==false)
            {
                m_shootableSpotPosition = FindASpotToShootFrom(3);
            }
        }
        
        //Move towards spot where the character can shoot from.
        Vector3 direction = pathFinder.GetMoveDirection(ownerPosition, m_shootableSpotPosition);
        GameManager.instance.GetEventManager().AddEvent(new AIMoveEvent(m_ownerCharacter, direction.normalized));

        //If reached the shootable spot,return success.
        if ((m_shootableSpotPosition - ownerPosition).sqrMagnitude < 1) return NodeState.Sucessful;


        return NodeState.Running;
    }


    private bool CheckIfCanShootTarget(Vector3 aOriginPosition,Character aTargetCharacter)
    {
        Ray ray=new Ray(aOriginPosition, aTargetCharacter.GetPositionOfHead());
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

    private Vector3 FindASpotToShootFrom(int aRadiusOfGridCellsToCheck)
    {
        List<Vector3> spotsToCheck=pathFinder.GetNeightbouringCellsPosition(m_ownerCharacter.GetPosition(), aRadiusOfGridCellsToCheck); //Get list of neighbouring cells to try and shoot from.
        
        List<Vector3> spotsToShootFrom=new List<Vector3>();

        Character targetCharacter = (Character)GetData(key_targetCharacter);

        Vector3 characterHeight = m_ownerCharacter.GetPositionOfHead()-m_ownerCharacter.GetPosition();

        //Check from each neighbouing cell if this character can shoot target and add that cell to a list.
        foreach (Vector3 spot in spotsToCheck)
        {
            if (CheckIfCanShootTarget(characterHeight + spot, targetCharacter))
                spotsToShootFrom.Add(spot);
        }
        
        //From the shootable list of cells find the clossest one.
        Vector3 nearestSpot=Vector3.zero;
        float nearestSpotDist = float.MaxValue;

        for(int i=0;i<spotsToShootFrom.Count;i++)
        {
            if ((m_ownerCharacter.GetPosition() - spotsToShootFrom[i]).sqrMagnitude < nearestSpotDist)
            {
                nearestSpot = spotsToShootFrom[i];
                nearestSpotDist = spotsToShootFrom[i].sqrMagnitude;
            }
        }

        return nearestSpot;
    }
}
