using UnityEngine;
namespace WeaponSystem
{
[CreateAssetMenu(fileName = "NewWeapn", menuName = "Weapons/RangedWeapon/CreateNewWeapon")]
public class RangedWeapon : WeaponBase
{
    [SerializeField] int m_clipSize;
    [SerializeField] int m_ammo;

    public void DeductAmmo() { m_ammo -= 1;Debug.Log(m_ammo); }
    public void SetAmmo(int amount) { m_ammo = Mathf.Clamp( amount,0, m_clipSize); }

    public int GetAmmo() { return m_ammo; }
    public int GetClipSize() { return m_clipSize; }

    public override WeaponHitResult Fire(LayerMask aRayMask, Ray ray)
    {
        if (aRayMask != null)
        {
            WeaponHitResult weponHitResult = null;
            if (m_ammo > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, aRayMask))
                {
                    Vector3 hitDirection=hit.point-ray.origin;
                    hitDirection.Normalize();
                    weponHitResult = new WeaponHitResult(hit.transform.gameObject, hit.point, hitDirection);
                }
            }
            return weponHitResult;
        }
        Debug.LogError("No Layermask");
        return null;
    }
}
}
