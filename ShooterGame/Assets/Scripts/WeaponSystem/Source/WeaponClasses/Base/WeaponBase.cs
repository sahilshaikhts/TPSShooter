using UnityEngine;

namespace WeaponSystem
{
public abstract class WeaponBase : ScriptableObject
{
    [SerializeField] protected string m_name;
    [SerializeField] protected Mesh m_mesh;
    [SerializeField] protected int m_damageAmount;
    [SerializeField] protected float m_fireRate;
    [SerializeField] protected float m_targetPushForce;

    public abstract WeaponHitResult Fire(LayerMask aRayMask, Ray ray);
    
    public string GetName() { return m_name; }
    public int GetDamageAmount() { return m_damageAmount; }
    public Mesh GetMesh() { return m_mesh; }
}

public class WeaponHitResult
{
    private GameObject m_hitObject;
    private Vector3 m_hitPosition;
    private Vector3 m_hitDirection;

    public WeaponHitResult(GameObject ahitObject, Vector3 aHitPosition)
    {
        m_hitObject = ahitObject;
        m_hitPosition = aHitPosition;
    }

    public WeaponHitResult(GameObject ahitObject, Vector3 aHitPosition, Vector3 aHitDirection)
    {
        m_hitObject = ahitObject;
        m_hitPosition = aHitPosition;
        m_hitDirection = aHitDirection;
    }

    void SetHitObject(GameObject ahitObject) { m_hitObject = ahitObject; }
    void SetHitPosition(Vector3 aHitPosition) { m_hitPosition = aHitPosition; }
    void SetHitDirection(Vector3 aHitDirection) { m_hitDirection = aHitDirection; }

    public GameObject GetHitObject() { return m_hitObject; }
    public Vector3 GetHitPosition() { return m_hitPosition; }
    public Vector3 GetHitDirection() { return m_hitDirection; }
}

}
