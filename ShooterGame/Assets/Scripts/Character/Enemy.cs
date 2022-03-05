using EventSystem;
using System;
using UnityEngine;
using WeaponSystem;

namespace ShootingGame
{
public class Enemy : Character, IShotable
{
    MovementComponent m_movementComponent;
    private void Start()
    {
        m_movementComponent = GetComponent<MovementComponent>();
        GameManager.GetEventManager().SubscribeToEvent(AIMoveEvent.EventType(), HandleMoveEvent);
    }

    private void HandleMoveEvent(IEvent aEvent)
    {
        AIMoveEvent moveEvent = (AIMoveEvent)aEvent;
        if (moveEvent.GetCallerGameObject() == this.gameObject)
        {
            m_movementComponent.SetMovementDirection(moveEvent.GetDirection());
        }
    }
     
    public void HandleGettingShot(Vector3 projetileDirection, int damageAmount)
    {
        m_movementComponent.ApplyForce(projetileDirection,20);
        DealDamage(damageAmount);
    }
}
}