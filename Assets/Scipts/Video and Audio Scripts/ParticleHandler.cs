using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    int delayToDestroyParticle = 3;
    

    public void SpawnParticle(ParticleSystem particleName, Vector3 particleSpawnPos, Quaternion particleRotation)
    {
        ParticleSystem particle;
        particle = Instantiate(particleName, particleSpawnPos, particleRotation) as ParticleSystem;
        particle.gameObject.AddComponent<ParticleHandler>();
        ParticleHandler particleHandler = particle.gameObject.GetComponent<ParticleHandler>();
        particleHandler.SelfDestroyParticle();
        
        
    }

    
    public void SelfDestroyParticle ()
    {
        Destroy(this.gameObject, delayToDestroyParticle);
    }
}
