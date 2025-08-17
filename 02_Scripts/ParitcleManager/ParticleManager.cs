using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [field: SerializeField] public List<ParticleData> ParticleDataList { get; private set; } = new();

    public Dictionary<ParticleType, ParticleSystem> ParticleSystem { get; private set; } = new();

    public void Awake()
    {
        base.Awake();
        foreach(var particle in ParticleDataList)
        {
            ParticleSystem.Add(particle.particleType, particle.particleSystem);
        }
    }

    public void StartParticle(ParticleType type, Transform pos)
    {
        ParticleSystem[type].transform.parent = pos;
        ParticleSystem[type].transform.localPosition = Vector3.zero;
        ParticleSystem[type].Play();
    }

    public void StopParticle(ParticleType type)
    {
        ParticleSystem[type].transform.parent = this.transform;
        ParticleSystem[type].Stop();
    }
}

public enum ParticleType
{
    Walk,
    Run,
}

[System.Serializable]
public class ParticleData
{
    public ParticleType particleType;
    public ParticleSystem particleSystem;
}
