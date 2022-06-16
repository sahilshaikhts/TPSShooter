using UnityEngine;
namespace WeaponSystem
{
[CreateAssetMenu(fileName = "NewWeapn", menuName = "Weapons/RangedWeapon/CreateNewWeapon")]
public class RangedWeapon : WeaponBase
{
    [SerializeField] int m_clipSize;
    [SerializeField] int m_ammo;

    [SerializeField]ProjectileFXData m_projectileFXData;

    public void Awake()
    {
        GameObject projectileObj = new GameObject("Projectile");
        projectileObj.tag = "projectile";

        SphereCollider collider;
        collider = projectileObj.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.01f;

        projectileObj.AddComponent<Projectile>();

        //Initilizing object pool with created projectile
        m_projectilePool = new ObjectPool();
        m_projectilePool.Initialize(projectileObj, 25);

        GameObject.Destroy(projectileObj);
    }
    public void DeductAmmo() { m_ammo -= 1; }
    public void SetAmmo(int amount) { m_ammo = Mathf.Clamp( amount,0, m_clipSize); }

    public int GetAmmo() { return m_ammo; }
    public int GetClipSize() { return m_clipSize; }

    public override void Fire(LayerMask aRayMask, Ray ray)
    {
        if (aRayMask != null)
        {
            WeaponHitResult weponHitResult = null;
            if (m_ammo > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, aRayMask))
                {
                    Vector3 hitDirection = hit.point - ray.origin;
                    hitDirection.Normalize();
                    weponHitResult = new WeaponHitResult(hit.transform.gameObject, hit.point, hitDirection);
                    IShotable shotableObject=hit.transform.GetComponent<IShotable>();

                    if(shotableObject != null)
                    shotableObject.HandleGettingShot(hitDirection,GetDamageAmount());

                    //ToDo: Do I really need the projectile class and spawning them??
                    //SpawnProjectile(weponHitResult);
                }
                else //If shot in air 
                {
                    //Just spawn projectile
                }

                DeductAmmo();
            }
        }
        else
        {
            Debug.LogError("No Layermask");
        }
    }

    public override void SpawnProjectile(WeaponHitResult aWeaponHitResult)
    {
        GameObject projectileObj = m_projectilePool.Spawn(aWeaponHitResult.GetHitPosition(),Quaternion.identity);

        projectileObj.GetComponent<Projectile>().Initzialize(aWeaponHitResult.GetHitPosition(), aWeaponHitResult.GetHitDirection(), GetDamageAmount());
        //projectileObj.GetComponent<Projectile>().m_projectileFXData=m_
        GameManager.instance.GetTimer().TimedExecution(DeactivateProjetile,0.1f);
    }

    public void DeactivateProjetile()
    {
        m_projectilePool.DeactivateFirstObject();
    }
}
}
