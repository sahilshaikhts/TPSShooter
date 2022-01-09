using UnityEngine;

namespace ShootingGame
{
public class Character : MonoBehaviour
{
    [SerializeField]int m_health,m_maxHealth;
 
    protected bool DealDamage(int amount)
    {
        m_health = Mathf.Clamp(m_health - amount, 0, m_maxHealth);

        if (m_health < 1) return false;

        return true;
    }

    public int GetHP() { return m_health; }
}
}
