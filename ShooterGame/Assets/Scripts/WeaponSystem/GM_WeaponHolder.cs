using UnityEngine;

namespace WeaponSystem
{
public class GM_WeaponHolder : WeaponHolderBase
{
    public GM_WeaponHolder(LayerMask aLayerMask) : base(aLayerMask) { }

    protected override void SpawnProjectile(WeaponHitResult aWeaponHitResult)
    {
        GameObject projectileObj = new GameObject("Projectile");
        projectileObj.tag = "projectile";
        projectileObj.transform.position = aWeaponHitResult.GetHitPosition();

        SphereCollider collider;
        collider = projectileObj.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.01f;

        projectileObj.AddComponent<Projectile>();
        projectileObj.GetComponent<Projectile>().Initzialize(aWeaponHitResult.GetHitPosition(), aWeaponHitResult.GetHitDirection(), m_weapons[m_currentWeaponIndex].GetDamageAmount());

        GameObject.Destroy(projectileObj, .1f);
    }

}
}
