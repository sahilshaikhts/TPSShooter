using UnityEngine;
using WeaponSystem;

namespace ShootingGame
{
public class Enemy : Character, IShotable
{
    PlayerController m_playerController;
    MovementComponent m_movementComponent;
    WeaponHolder m_weaponHolder;

    private void Start()
    {
        m_movementComponent = GetComponent<MovementComponent>();
    }

    public void HandleGettingShot(Vector3 projetileDirection, int damageAmount)
    {
        m_movementComponent.ApplyForce(projetileDirection,20);
        DealDamage(damageAmount);
    }
}
}