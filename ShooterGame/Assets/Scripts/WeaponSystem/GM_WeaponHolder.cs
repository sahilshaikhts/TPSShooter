using System.Collections;
using UnityEngine;










///
///
//Todo: To delete the projectile after a time ,put Destroy code in the init of the projectle
///
///






namespace WeaponSystem
{
public class GM_WeaponHolder : WeaponHolderBase
{
    ObjectPool m_projectilePool;

    public GM_WeaponHolder(LayerMask aLayerMask) : base(aLayerMask) 
    {
        //Create a projectile prefab for ObjectPool
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

    protected override void SpawnProjectile(WeaponHitResult aWeaponHitResult)
    {
        GameObject projectileObj = m_projectilePool.Spawn(aWeaponHitResult.GetHitPosition(),Quaternion.identity);

        projectileObj.GetComponent<Projectile>().Initzialize(aWeaponHitResult.GetHitPosition(), aWeaponHitResult.GetHitDirection(), m_weapons[m_currentWeaponIndex].GetDamageAmount());

        GameManager.GetTimer().TimedExecution(DeactivateProjetile,0.1f);
    }

    void DeactivateProjetile()
    {
        m_projectilePool.DeactivateFirstObject();
    }

}
}
