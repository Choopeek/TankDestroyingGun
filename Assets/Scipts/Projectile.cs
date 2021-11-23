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


    

    

    private void Awake()
    {
        projectileRB = this.GetComponent<Rigidbody>();
        //StartCoroutine(SelfDestruct());
        
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

    private void OnTriggerEnter(Collider other)
    {
                

        if (other.tag.Contains("Tank")) 
        {
            if (other.tag.Contains("Chassis"))
            {
                targetTankSCR = other.gameObject.GetComponent<Tank>();
            }

            else
            {
                targetTankSCR = other.gameObject.GetComponentInParent<Tank>();
            }
            
            target = other.gameObject;
            partThatWasHit = other.tag;
            targetTankSCR.Hit(target, whoShot, partThatWasHit, damage, penetration);
            
        }
        

        if (other.CompareTag("Untagged"))
        {
            Debug.Log("Hit an UNTAGGED obj");
        }



            Destroy(gameObject);
    }

    


}
