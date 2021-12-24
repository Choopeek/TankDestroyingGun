using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tank : MonoBehaviour
{
    GameManager gameManager;
    GameObject trackLeftGO;
    GameObject trackRightGO;
    private Scroll_Track trackLeft;
    private Scroll_Track trackRight;

    public GameObject chassis;
    public GameObject turret;
    [SerializeField] public  GameObject gun;
    public GameObject gunEdge;    
    PlayerCombatModule playerCM;
    
    
    float gunAngle;
    bool gunCanMoveUp;
    bool gunCanMoveDown;

    Rigidbody tankRigidBody;

    

    public TankClass tank = new TankClass();
    public CombatSystem combatSystem = new CombatSystem();

    public ParticleCatalogue particleCatalogue = new ParticleCatalogue();
    ParticleHandler particleHandler;


    public Material material;



    
    private void Start()
    {
        HandleModel(); //in this function we make all that stuff that is usually made in START method;        
        HandleProjectile();
        HandleVFX();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    #region Handling A Tank //setting up so the model would work (components, scripts, etc)
    void HandleModel()
    {
        //setting Childs and adding Scripts to tracks
        chassis = this.transform.gameObject;
        trackLeftGO = this.transform.GetChild(0).gameObject;
        trackRightGO = this.transform.GetChild(1).gameObject;
        this.transform.GetChild(0).gameObject.AddComponent<Scroll_Track>();
        this.transform.GetChild(1).gameObject.AddComponent<Scroll_Track>();
        trackLeft = this.transform.GetChild(0).GetComponent<Scroll_Track>();
        trackRight = this.transform.GetChild(1).GetComponent<Scroll_Track>();
        
        turret = this.transform.GetChild(2).gameObject;

        //setting stuff about mainGun
        gun = this.transform.GetChild(2).GetChild(0).gameObject;                
        gunEdge = gun.transform.GetChild(0).gameObject;

        //Handling RigidBody component
        tankRigidBody = this.GetComponent<Rigidbody>();
        tankRigidBody.mass = tank.weight;

        
    }

    void HandleProjectile()
    {
        playerCM = GameObject.Find("Player").GetComponent<PlayerCombatModule>();
        tank.mainGunProjectile = playerCM.projectileForEnemy;
    }

    void HandleVFX()
    {
        particleHandler = this.gameObject.AddComponent<ParticleHandler>();

    }

    #endregion


    #region Movement
    public void MoveTankForward()
    {
        if (!IsAlive())
        {
            return;
        }
        this.transform.Translate(Vector3.forward * this.tank.forwardSpeed * Time.deltaTime);        
        trackLeft.movingForward = true;
        trackRight.movingForward = true;
    }

    public void MoveTankBackwards()
    {
        if (!IsAlive())
        {
            return;
        }
        this.transform.Translate(Vector3.back * this.tank.backwardSpeed * Time.deltaTime);
        trackLeft.movingBackward = true;
        trackRight.movingBackward = true;
    }

    public void StopTracks()
    {
        
        trackRight.StoppedMoving();
        trackLeft.StoppedMoving();
    }
    public void RotateTankLeft(bool rotateLeft)
    {
        if (!IsAlive())
        {
            return;
        }
        if (rotateLeft)
        {
            this.transform.Rotate(0f, -this.tank.turnSpeed * Time.deltaTime, 0f, Space.Self);
            trackLeft.movingBackward = true;
            trackRight.movingForward = true;
        }

        else
        {
            this.transform.Rotate(0f, this.tank.turnSpeed * Time.deltaTime, 0f, Space.Self);
            trackLeft.movingForward = true;
            trackRight.movingBackward = true;
        }

        
    }

    public void RotateTurretLeft(bool rotateLeft)
    {
        if (!IsAlive())
        {
            return;
        }
        if (rotateLeft)
        {
            this.turret.transform.Rotate(0f, -this.tank.turretTurnSpeed * Time.deltaTime, 0f, Space.Self);
        }

        else
        {
            this.turret.transform.Rotate(0f, this.tank.turretTurnSpeed * Time.deltaTime, 0f, Space.Self);
        }
    }

    public void MoveGunUp(bool moveUp)
    {
        if (!IsAlive())
        {
            return;
        }

        if (moveUp)
        {
            GunIsInBounds();  
            if (gunCanMoveUp)
            {
                this.gun.transform.Rotate(-tank.gunMoveSpeed * Time.deltaTime, 0f, 0f, Space.Self);
            }
            
            else
            {
                return;
            }
            
        }

        if (!moveUp)
        {
            GunIsInBounds();
            if (gunCanMoveDown)
            {
                this.gun.transform.Rotate(tank.gunMoveSpeed * Time.deltaTime, 0f, 0f, Space.Self);
            }

            else
            {
                return;
            }
            
        }

        
    }

    #endregion
    public void FireMainGun()
    {
        if (!IsAlive())
        {
            return;
        }
        Transform shootPos;
        shootPos = gunEdge.transform;
        
        GameObject shotFired;
        Projectile projectileSCR;
        shotFired = Instantiate(tank.mainGunProjectile, shootPos.position, shootPos.rotation) as GameObject;
        projectileSCR = shotFired.GetComponent<Projectile>();
        projectileSCR.ShootProjectile(tank.mGProjectileVelocity);
        projectileSCR.whoShot = this.gameObject;
        projectileSCR.penetration = combatSystem.projectilePenetrationValue(tank.mGMinPenetration, tank.mGGunMaxPenetration);
        projectileSCR.damage = tank.mGDamage;

        
        
        particleHandler.SpawnParticle(particleCatalogue.shootSmokeMainGun, shootPos.position, shootPos.rotation);
    }

    //checks whenever the gun is in bounds of the Angles; If so - it changes the variables, so the gun can't be moved up or down anymore;
    void GunIsInBounds()
    {
        gunAngle = this.gun.transform.localEulerAngles.x;
        gunCanMoveDown = true;
        gunCanMoveUp = true;

        if (gunAngle >= this.tank.gunDownBond & gunAngle <= this.tank.gunUpBond)
        {
            if (gunAngle <= this.tank.gunDownBond + 180)
            {
                gunCanMoveUp = true;
                gunCanMoveDown = false;
            }

            else
            {
                gunCanMoveDown = true;
                gunCanMoveUp = false;
            }
        }

        else
        {
            gunCanMoveDown = true;
            gunCanMoveUp = true;
        }
    }



    #region CombatMechanic
    
    public void Hit(GameObject target, GameObject whoShot, string partThatWasHit, int projectileDamage, int projectilePenetration, Vector3 hitCoordinates, ParticleSystem penetrationParticle, ParticleSystem notPenetratedParticle)
    {
        if (!IsAlive())
        {
            return;
        }
        int[] armorValues = ArmorThicknessThatWasHit(partThatWasHit);

        if (combatSystem.GotHit(target, whoShot, projectilePenetration, armorValues))
        {
            particleHandler.SpawnParticle(penetrationParticle, hitCoordinates, penetrationParticle.transform.rotation);
            TakeDamage(projectileDamage);
        }
        else
        {
            particleHandler.SpawnParticle(notPenetratedParticle, hitCoordinates, notPenetratedParticle.transform.rotation);
            return;
        }
    }

    void TakeDamage(int damage)
    {
        tank.hitPoints = tank.hitPoints - damage;
        if (tank.hitPoints <= 0)
        {
            DestroyTank();
        }
    } 

    public void DestroyTank()
    {
        gameManager.ObjectWasDestroyed(this.gameObject, tank.scoreValue);
        StopTracks();
        SetMaterialOnDeath();
        Destroy(gameObject, 5);
    }

    
    void SetMaterialOnDeath()
    {
        Material chassisNewMaterial;
        Material trackLeftNewMaterial;
        Material trackRightNewMaterial;
        Material turretNewMaterial;
        Material gunNewMaterial;

        chassisNewMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
        trackLeftNewMaterial = this.trackLeftGO.GetComponent<MeshRenderer>().material;
        trackRightNewMaterial = this.trackRightGO.GetComponent<MeshRenderer>().material;
        turretNewMaterial = this.turret.GetComponent<MeshRenderer>().material;
        gunNewMaterial = this.gun.GetComponent<MeshRenderer>().material;

        chassisNewMaterial.color = new Color(0, 0, 0);
        trackLeftNewMaterial.color = new Color(0, 0, 0);
        trackRightNewMaterial.color = new Color(0, 0, 0);
        turretNewMaterial.color = new Color(0, 0, 0);
        gunNewMaterial.color = new Color(0, 0, 0);

        
    }

    

    //returns the array of armorValues
    int[] ArmorThicknessThatWasHit(string partThatWasHit)
    {
        int frontArmor = 0;
        int sideArmor = 0;
        int rearArmor = 0;

        if (partThatWasHit.Contains("Chassis"))
        {
            frontArmor = tank.chassisFrontArmor;
            sideArmor = tank.chassisSideArmor;
            rearArmor = tank.chassisRearArmor;
        }

        if (partThatWasHit.Contains("Turret"))
        {
            frontArmor = tank.turretFrontArmor;
            sideArmor = tank.turretSideArmor;
            rearArmor = tank.turretRearArmor;
        }

        if (partThatWasHit.Contains("Track"))
        {
            frontArmor = tank.tracksArmor;
            sideArmor = tank.tracksArmor;
            rearArmor = tank.tracksArmor;
        }

        if (partThatWasHit.Contains("TankGun"))
        {
            frontArmor = tank.gunArmor;
            sideArmor = tank.gunArmor;
            rearArmor = tank.gunArmor;
        }        

        int[] partArmor = new int[3];
        partArmor[0] = frontArmor;
        partArmor[1] = sideArmor;
        partArmor[2] = rearArmor;

        return partArmor;
            
    }


    #endregion

    #region Video effects

    

    

    #endregion

    //small method to know if smth happened in percents chance
   public bool DidItHappened(int percentChance) 
    {
        int chance = UnityEngine.Random.Range(0, 101);
        if (percentChance >= chance)
        {
            Debug.Log("It happened, the chance was " + percentChance);
            return true;
        }
        else
        {
            Debug.Log("it did not happened, the chance was " + percentChance);
            return false;
        }

    }

    bool IsAlive()
    {
        if (tank.hitPoints > 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}
