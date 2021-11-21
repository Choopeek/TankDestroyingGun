using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TankClass
{
    [SerializeField] public float weight;

    [Header("Speed Values")]
    [SerializeField] public float forwardSpeed;
    [SerializeField] public float backwardSpeed;
    [SerializeField] public float turnSpeed;
    [SerializeField] public float turretTurnSpeed;
    [Space(20)]

    [Header("Gun Movement Values")]
    [SerializeField] public float gunMoveSpeed;

    [SerializeField] public float gunUpBond;
    [SerializeField] public float gunDownBond;
    
    //used for turretless TankDestroyers etc. 
    //M3 Lee will be a problem;
    [SerializeField] public float gunLeftBond;
    [SerializeField] public float gunRightBond;
    [Space(10)]


    [Header("Gun Damage Control")]
    //addArmor here, damage and all that stuff when you implement it.
    [SerializeField] public GameObject mainGunProjectile;
    [SerializeField] public float mGProjectileVelocity;
    [SerializeField] public float mainGunDamage;
    [SerializeField] public float mGReloadSpeed;
    [SerializeField] public float mGMagazineSize;
    [SerializeField] public float mGMinimumPenetration;
    [SerializeField] public float mGGunMaximumPenetration;
    [SerializeField] public float hitPoints;

   

}
