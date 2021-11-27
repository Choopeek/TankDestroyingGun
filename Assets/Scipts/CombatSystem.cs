using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem
{
   //CombatSystem is used to handle all combat related methods. Maybe it will be a nice idea to make in Monobehaviour. So we can call all this stuff more easily.
    
    

    //GotHit method defines - was the target hit successfully. Checks the penetration and armor values depending on the targets angle that the shot was take from
    public bool GotHit(GameObject target, GameObject whoShot, int projectilePenetration, int[] armorValues)
    {
        float angleBetween = GetAngle(target, whoShot);
        int armor = GetArmorValue(armorValues, angleBetween);
        if (WasPenetrated(armor, projectilePenetration))
        {
            Debug.Log("Penetrated");
            return true;

            
        }
        else
        {
            Debug.Log("Not penetrated");
            return false;
            

        }



        
    }


    //GetAngle defines the angle between the target and the attacker. It is used to help pick the corresponding armor value. It is an oversimplification of the collider based armor. But still it works. And it saves A LOT of time, if I mannually placed collider on models;
    float GetAngle(GameObject whatWasHit, GameObject whoMadeShot)
    {
        float angleBetween;
        Vector3 targetDir = whoMadeShot.transform.position - whatWasHit.transform.position;
        angleBetween = Vector3.Angle(targetDir, whatWasHit.transform.forward);
        return angleBetween;
    }


    //called from the Script of the Atacking vehicle
    public int projectilePenetrationValue(int minPenetration, int maxPenetration)
    {
        int penetration = Random.Range(minPenetration, maxPenetration);
        return penetration;
    }

    //called from the Script of the Target vehicle
    int GetArmorValue(int[] armorValues, float angleBetween)
    {
        int armor = 0;

        if (angleBetween <= 45)
        {
            Debug.Log("Hit in front");
            armor = armorValues[0];
            
        }
        if (angleBetween > 45 & angleBetween <= 135)
        {
            Debug.Log("Hit in side");
            armor = armorValues[1];
            
        }
        if (angleBetween > 135)
        {
            Debug.Log("Hit in back");
            armor = armorValues[2];
            
        }

        return armor;
    }

    //checks if the armor was penetradet by the projectiles penetration value
    bool WasPenetrated(int armor, int penetration)
    {
        if (armor > penetration)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    

    

    





}
