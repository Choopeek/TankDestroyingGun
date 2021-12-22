using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody projectileRB;
    public int damage;
    public int penetration;
    

    public GameObject whoShot;
    Tank tankSCR;
    public GameObject target;
    Tank targetTankSCR;
    [SerializeField] string partThatWasHit;

    ParticleHandler particleHandler;
    [SerializeField] ParticleSystem penetrationParticle;
    [SerializeField] ParticleSystem notPenetratedParticle;

    

    

    //use Projectile RB as a Continious Dynamic type. Otherwise fast projectiles won't be detected by the colliders. Also you can try RayCasting.

    private void Awake()
    {
        projectileRB = this.GetComponent<Rigidbody>();
        StartCoroutine(SelfDestruct());
        this.gameObject.AddComponent<ParticleHandler>();
        particleHandler = this.gameObject.GetComponent<ParticleHandler>();
        
        
    }

    public void ShootProjectile(float velocity)
    {        
        projectileRB.AddRelativeForce(0, 0, velocity, ForceMode.VelocityChange);
    }

    

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 hitCoordinates;
        hitCoordinates = collision.GetContact(0).point;

        NavigationManagerAgent botIsShooting;
        try
        {
            botIsShooting = whoShot.GetComponent<NavigationManagerAgent>();
            
        }
        catch (System.Exception)
        {
            Debug.Log("Something is wrong in the projectile method. OnColisionEnter ");
            throw;
        }

        

        //turret and chassis have different armor values. So here we help the projectile to know - what was hit. And pass this info to Targets script. And based on that knowledge - it will pass the correct armor values;
        if (collision.gameObject.tag.Contains("Tank"))
        {
            

            if (collision.gameObject.tag.Contains("Chassis"))
            {
                targetTankSCR = collision.gameObject.GetComponent<Tank>();
            }

            else
            {
                targetTankSCR = collision.gameObject.GetComponentInParent<Tank>();
                
                
            }

            target = collision.gameObject;
            partThatWasHit = collision.gameObject.tag;
            

            targetTankSCR.Hit(target, whoShot, partThatWasHit, damage, penetration, hitCoordinates, penetrationParticle, notPenetratedParticle);
            //ERRORS MIGHT BE HERE WITH THE NavigationManagerAgent
            if (botIsShooting != null)
            {
                try
                {
                    botIsShooting.objectThatWasHit = targetTankSCR.chassis;
                }
                catch
                {
                    Debug.Log("exception in OnColisionEnter   if (collision.gameObject.tag.Contains(Tank))");
                    throw;
                }
            }
            


        }

        if (botIsShooting != null)
        { //ERRORS MIGHT BE HERE WITH THE NavigationManagerAgent
            
            botIsShooting.hitCoordinates = hitCoordinates;
            botIsShooting.awaitingExplosionCoordinates = false;
        }

        if (collision.gameObject.CompareTag("Untagged"))
        {         
            particleHandler.SpawnParticle(penetrationParticle, hitCoordinates, transform.rotation);
        }

        

        Destroy(gameObject);
    }
    

    
    

}
