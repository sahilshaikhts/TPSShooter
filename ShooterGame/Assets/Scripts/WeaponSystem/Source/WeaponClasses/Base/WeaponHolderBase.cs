using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
public abstract class WeaponHolderBase
{
    protected List<WeaponBase> m_weapons;
    protected int m_currentWeaponIndex = 0;
    protected LayerMask m_layermask;

    public WeaponHolderBase(LayerMask aLayerMask)
    {
        m_layermask = aLayerMask;
        m_weapons = new List<WeaponBase>();
    }

    public void Fire(Ray ray)
    {
        if (m_weapons.Count == 0) return;
        m_weapons[m_currentWeaponIndex].Fire(m_layermask, ray);
    }

    public void StepWeapon(int stepDirection)
        {
            if (m_weapons.Count > 1)
            {
                m_currentWeaponIndex += stepDirection;
                m_currentWeaponIndex = Mathf.Clamp(m_currentWeaponIndex, 0, m_weapons.Count - 1);
            }
        }

    public void AddNewWeapon(WeaponBase aNewweapon)
    {
            //Todo: Add check to avoid duplication and instantiate new instance of weapon
        m_weapons.Add(aNewweapon);
    }
}

public class Projectile : MonoBehaviour
{
    Vector3 m_hitDirection;
    int m_damageAmount = 0;

    public ProjectileFXData m_projectileFXData;

    private void OnEnable()
    {
        m_hitDirection = transform.forward;
    }

    public void Initzialize(Vector3 aHitPosition, Vector3 aHitDirection, int aDamageAmount)
    {
        m_hitDirection = aHitDirection;
        m_damageAmount = aDamageAmount;
        GetComponent<SphereCollider>().enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerProjectileParticle(m_projectileFXData.GetProjectileParticleEffect());
        TriggerSurfaceParticleEffect(m_projectileFXData.GetSurfaceParticleEffect());
        TriggerSoundEffect(m_projectileFXData.GetSoundEffect());

        if (other.GetComponent<IShotable>() != null)
        {
            other.GetComponent<IShotable>().HandleGettingShot(m_hitDirection, m_damageAmount);
        }
        GetComponent<SphereCollider>().enabled = false;
    }


    void TriggerProjectileParticle(ParticleSystem particleEffect)
    {
        if (particleEffect)
            particleEffect.Play();
    }

    void TriggerSurfaceParticleEffect(ParticleSystem particleEffect)
    {
        if (particleEffect)
            particleEffect.Play();
    }

    void TriggerSoundEffect(AudioClip audioClip)
    {

    }
}

}

