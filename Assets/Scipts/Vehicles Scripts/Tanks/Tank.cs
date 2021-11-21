using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    private Scroll_Track trackLeft;
    private Scroll_Track trackRight;

    GameObject turret;
    [SerializeField] GameObject gun;
    GameObject gunEdge;    
    PlayerCombatModule playerCM;
    
    
    float gunAngle;
    bool gunCanMoveUp;
    bool gunCanMoveDown;

    Rigidbody tankRigidBody;

    

    public TankClass tank = new TankClass();

    private void Start()
    {
        HandleModel(); //in this function we make all that stuff that is usually made in START method;        
        HandleProjectile();
    }

    #region Handling A Tank //setting up so the model would work
    void HandleModel()
    {
        //setting Childs and adding Scripts to tracks
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

    #endregion


    #region Movement
    public void MoveTankForward()
    {
        this.transform.Translate(Vector3.forward * this.tank.forwardSpeed * Time.deltaTime);
        trackLeft.movingForward = true;
        trackRight.movingForward = true;
    }

    

    public void MoveTankBackwards()
    {
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
        
        Transform shootPos;
        shootPos = gunEdge.transform;
        
        GameObject shotFired;
        Projectile projectileSCR;
        shotFired = Instantiate(tank.mainGunProjectile, shootPos.position, shootPos.rotation) as GameObject;
        projectileSCR = shotFired.GetComponent<Projectile>();
        projectileSCR.ShootProjectile(tank.mGProjectileVelocity);
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

    
}