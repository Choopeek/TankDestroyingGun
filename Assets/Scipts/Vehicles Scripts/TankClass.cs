using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TankClass
{
    [SerializeField] public int weight;
    [SerializeField] public int hitPoints;
    [SerializeField] public int scoreValue;

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
    [SerializeField] public int mGProjectileVelocity;
    [SerializeField] public int mGDamage;
    [SerializeField] public int mGReloadSpeed;
    [SerializeField] public int mGMagazineSize;
    [SerializeField] public int mGMinPenetration;
    [SerializeField] public int mGGunMaxPenetration;
    [Space(10)]

    [Header("Chassis armor")]
    [Space(2)]
    [Header("Armor values")]
    [SerializeField] public int chassisFrontArmor;
    [SerializeField] public int chassisSideArmor;
    [SerializeField] public int chassisRearArmor;
    [Space(2)]
    [Header("Turret armor")]
    [SerializeField] public int turretFrontArmor;
    [SerializeField] public int turretSideArmor;
    [SerializeField] public int turretRearArmor;
    [Space(2)]
    [Header("Other armor")]
    [SerializeField] public int tracksArmor;
    [SerializeField] public int gunArmor;





}
