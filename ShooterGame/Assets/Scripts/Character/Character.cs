using UnityEngine;

namespace ShootingGame
{
public class Character : MonoBehaviour
{
    [SerializeField]int m_health,m_maxHealth;
    [SerializeField]MeshRenderer m_Renderer;

    protected bool DealDamage(int amount)
    {
        m_health = Mathf.Clamp(m_health - amount, 0, m_maxHealth);

        if (m_health < 1) return false;

        return true;
    }
    public Vector3 GetPosition() { return transform.position; }
    public int GetHP() { return m_health; }

    public Vector3 GetPositionOfHead()
    {
        Vector3 headPosition = transform.position;
        headPosition.y += m_Renderer.bounds.size.y / 2;

        return headPosition;
    }
    public int GetCharacterLayer() { return gameObject.layer; }
}
}
