using EventSystem;
using System;
using UnityEngine;
using WeaponSystem;

namespace ShootingGame
{
public class Enm_luminousCentaurs : Character, IShotable
{
    MovementComponent m_movementComponent;
    WeaponHolder m_weaponHolder;

    [SerializeField]WeaponBase tmp_pistol;
    [SerializeField]LayerMask m_weaponLayerMask;


    private void Start()
    {   
        m_movementComponent = GetComponent<MovementComponent>();
        m_weaponHolder = new WeaponHolder(m_weaponLayerMask);

        WeaponBase pistol = Instantiate(tmp_pistol);
        m_weaponHolder.AddNewWeapon(pistol);

        GameManager.instance.GetEventManager().SubscribeToEvent(AIMoveEvent.EventType(), HandleMoveEvent);
        GameManager.instance.GetEventManager().SubscribeToEvent(AIShootTargetEvent.EventType(), HandleShootTargetEvent);
    }

    private void HandleMoveEvent(IEvent aEvent)
    {
        AIMoveEvent moveEvent = (AIMoveEvent)aEvent;
        if (moveEvent.GetCaller() == this)
        {
            m_movementComponent.SetMovementDirection(moveEvent.GetDirection());
        }
    }
     
    private void HandleShootTargetEvent(IEvent aEvent)
    {
        AIShootTargetEvent moveEvent = (AIShootTargetEvent)aEvent;
        if (moveEvent.GetCaller() == this)
        {
            Ray ray = new Ray(GetPositionOfHead(), moveEvent.GetDirection());
            m_weaponHolder.Fire(ray);
        }
    }

    public void HandleGettingShot(Vector3 projetileDirection, int damageAmount)
    {
        m_movementComponent.ApplyForce(projetileDirection,20);
        DealDamage(damageAmount);
    }
}
}