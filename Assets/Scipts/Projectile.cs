using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody projectileRB;
    public float damage;
    public float penetration;
    public float speed;

    

    private void Awake()
    {
        projectileRB = this.GetComponent<Rigidbody>();
        StartCoroutine(SelfDestruct());
        
    }

    public void ShootProjectile(float velocity)
    {
        speed = velocity;
        projectileRB.AddRelativeForce(0, 0, speed, ForceMode.VelocityChange);
    }

    public void TEST()
    {
        Debug.Log("projectile PEW PEW");
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
