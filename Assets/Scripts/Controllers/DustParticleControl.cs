using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleControl : MonoBehaviour
{
    [SerializeField] private bool createDuskOnWalke = true;
    [SerializeField] private ParticleSystem dustParticleSystem;

    public void CreateDustParticles()
    {
        if(createDuskOnWalke)
        {
            dustParticleSystem.Stop();
            dustParticleSystem.Play();
        }
    }
}
