using UnityEngine;

namespace WeaponSystem
{
public class WeaponHolder : WeaponHolderBase
{
    public WeaponHolder(LayerMask aLayerMask) : base(aLayerMask) { }

    //TODO override this funtion with a version using ObjecPool
    protected override void SpawnProjectile(WeaponHitResult aWeaponHitResult)
    {   
        //sapawn a dummy projectile,use it to compare the tag in OnCollision and handle response locally in the object's script
        GameObject projectile = new GameObject("Projectile");
        projectile.tag = "projectile";
        projectile.transform.position = aWeaponHitResult.GetHitPosition();

        SphereCollider collider;
        collider = projectile.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.01f;

        GameObject.Destroy(projectile, 0.1f);
    }
}
}

