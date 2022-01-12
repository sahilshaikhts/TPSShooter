using System.Collections;
using UnityEngine;

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
        GameObject projectileObj = new GameObject("Projectile");
        projectileObj.tag = "projectile";

//        GameObject obj= m_projectilePool.Spawn(aWeaponHitResult.GetHitPosition(),Quaternion.identity);

        projectileObj.GetComponent<Projectile>().Initzialize(aWeaponHitResult.GetHitPosition(), aWeaponHitResult.GetHitDirection(), m_weapons[m_currentWeaponIndex].GetDamageAmount());

        //MonoBehaviour.StartCoroutine(DeactivateProjetile(obj));
    }

    IEnumerator DeactivateProjetile(GameObject aObject)
    {
        yield return new WaitForSeconds(0.1f);

        m_projectilePool.DeactivateObject(aObject);
    }

}
}
