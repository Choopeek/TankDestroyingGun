using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem
{
   
    
    

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

    float GetAngle(GameObject whatWasHit, GameObject whoMadeShot)
    {
        float angleBetween;
        Vector3 targetDir = whoMadeShot.transform.position - whatWasHit.transform.position;
        angleBetween = Vector3.Angle(targetDir, whatWasHit.transform.forward);
        return angleBetween;
    }



    public int projectilePenetrationValue(int minPenetration, int maxPenetration)
    {
        int penetration = Random.Range(minPenetration, maxPenetration);
        return penetration;
    }

    int GetArmorValue(int[] armorValues, float angleBetween)
    {
        int armor = 0;

        if (angleBetween <= 45)
        {
            Debug.Log("Hit in front");
            armor = armorValues[0];
        }
        if (angleBetween >= 45 & angleBetween <= 135)
        {
            Debug.Log("Hit in side");
            armor = armorValues[1];
        }
        if (angleBetween >= 135)
        {
            Debug.Log("Hit in back");
            armor = armorValues[2];
        }

        return armor;
    }

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
