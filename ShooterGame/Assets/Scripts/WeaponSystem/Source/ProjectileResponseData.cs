using UnityEngine;

namespace WeaponSystem
{
public class ProjectileFXData : ScriptableObject
{
    [SerializeField] ParticleSystem m_projectleParticleEffect;
    [SerializeField] ParticleSystem m_surfaceParticleEffect;
    [SerializeField] AudioClip m_sfx;

    public ParticleSystem GetProjectileParticleEffect() { return m_projectleParticleEffect; }
    public ParticleSystem GetSurfaceParticleEffect() { return m_surfaceParticleEffect; }
    public AudioClip GetSoundEffect() { return m_sfx; }
}
}

