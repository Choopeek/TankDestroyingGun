using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NavigationManagerAgent : MonoBehaviour
{
    public List<Transform> waypointList = new List<Transform>();

    public GameObject tankToMove;
    Tank controlledTank;

    public GameObject target;
    GameObject turret;
    GameObject gun;

    public bool awaitingExplosionCoordinates;
    public bool awaitingGunAdjustment;
    public Vector3 hitCoordinates;
    public GameObject objectThatWasHit;

    float gunVerticalAdjustmentUpperBond = 90f;
    float gunVerticalAdjustmentLowerBond = 82f;

    float horizontalAdjustmentUpperBond = 90.9f;
    float horizontalAdjustmentLowerBond = 89.9f;

    public void GetScript()
    {
        if (tankToMove == null)
        {
            Debug.Log("NoTankToControll");
            return;
        }
        controlledTank = tankToMove.GetComponent<Tank>();
        turret = controlledTank.turret;
        gun = controlledTank.gun;
    }



    #region MovingMethods 
    void MoveForward(Transform moveFrom, Transform moveTo)
    {
        float distance = Vector3.Distance(moveFrom.position, moveTo.position);
        if (!IsOnSpot(moveFrom, moveTo))
        {
            controlledTank.MoveTankForward();
        }

        else
        {
            controlledTank.StopTracks();
            return;
        }





    }



   
    void RotateTowardsVersion2(Transform objectToMove, Transform targetObject)
    {
        float angle = GetAngleRight(objectToMove, targetObject);
        if (angle > 90)
        {
            controlledTank.RotateTankLeft(false);
            //rotateRight
        }

        if (angle < 90)
        {
            controlledTank.RotateTankLeft(true);
            //rotateLeft
        }

        if (angle >= horizontalAdjustmentLowerBond && angle <= horizontalAdjustmentUpperBond)
        {
            //isRotated
            Debug.Log("IsRotated");
        }

    }

    bool IsRotatedAtPointVersion2(Transform angleFrom, Transform angleTo)
    {
        float angle = GetAngleRight(angleFrom, angleTo);
        if (angle >= horizontalAdjustmentLowerBond && angle <= horizontalAdjustmentUpperBond)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float GetRotationDifference(Transform from, Transform to)
    {
        Vector3 targetDirection = (to.position - from.position).normalized;
        float dotProd = Vector3.Dot(targetDirection, from.forward);
        //Debug.Log(dotProd);
        return dotProd;
    }

    bool IsOnSpot(Transform yourLocation, Transform targetLocation)
    {
        if (GetDistance(yourLocation, targetLocation) < 3)
        {

            return true;
        }

        else
        {
            return false;
        }

    }

    float GetDistance(Transform from, Transform to)
    {
        float distance = Vector3.Distance(from.position, to.position);
        return distance;
    }

    float GetAngle(Transform takeAngleFrom, Transform takeAngleTo)
    {
        float angle = 0;

        Vector3 targetDir = takeAngleFrom.position - takeAngleTo.position;
        angle = Vector3.Angle(targetDir, takeAngleFrom.forward);
        return angle;


    }

    float GetAngleRight(Transform takeAngleFrom, Transform takeAngleTo)
    {
        float angle = 0;
        Vector3 targetDir = takeAngleFrom.position - takeAngleTo.position;
        angle = Vector3.Angle(targetDir, takeAngleFrom.right);
        return angle;
    }

    #endregion

    #region AimingMethods

    void RotateTurret()
    {        
            float angle = GetAngleRight(turret.transform, target.transform);
            if (angle > 90)
            {
                controlledTank.RotateTurretLeft(false);
                //rotateRight
            }

            if (angle < 90)
            {
                controlledTank.RotateTurretLeft(true);
                //rotateLeft
            }

            if (angle >= horizontalAdjustmentLowerBond && angle <= horizontalAdjustmentUpperBond)
            {
                //isRotated
                Debug.Log("IsRotated");
            }        
    }

    void ElevateGun(bool moveGunUp)
    {
        if (moveGunUp)
        {
            controlledTank.MoveGunUp(true);
        }

        if (!moveGunUp)
        {
            controlledTank.MoveGunUp(false);
        }
    }

    bool GunIsOnTarget()
    {
       float angleBetween = GetAngleForGun();
        if (angleBetween < gunVerticalAdjustmentUpperBond & angleBetween > gunVerticalAdjustmentLowerBond)
        {
            
            return true;
        }
        else
        {
            
            return false;
        }
    }

    float GetAngleForGun()
    {
               
        Vector3 targetDir = gun.transform.position - target.transform.position;
        float angle = Vector3.Angle(targetDir, gun.transform.up);        
        return angle;
    }

    #endregion


    //here all the movement works. The Tank will rotate and move towards all waypoints that are in his waypointList. After it - it stops.
    //eventually I got rid of Task Workflow, cause I did not want to mess with the cancelattion tokens. And now we stick with Couroutines.
    #region Couroutine Movement Workflow
    IEnumerator RotateAndMoveCor(Transform moveFrom, Transform moveTo)
    {

        while (!IsRotatedAtPointVersion2(moveFrom, moveTo))
        {
            RotateTowardsVersion2(moveFrom, moveTo);
            yield return null;
        }

        controlledTank.StopTracks();

        while (!IsOnSpot(moveFrom, moveTo))
        {
            MoveForward(moveFrom, moveTo);
            yield return null;
        }
        controlledTank.StopTracks();
        waypointList.RemoveAt(0);

        StartCoroutine(FollowThePathCor());
    }

    public IEnumerator FollowThePathCor()
    {
        if (!WaypointToMoveTo())
        {
            Debug.Log("reachedFinalDestination");            
            yield return null;

        }


        else
        {
            Transform fromCoord = tankToMove.transform;
            Transform toCoord = waypointList[0];
            StartCoroutine(RotateAndMoveCor(fromCoord, toCoord));
        }
    }
    #endregion

    #region Task Movement Workflow

    //in task workflow I did not plant any cancelation tokens. Its better for you to do so.
    async Task RotateAndMove(Transform moveFrom, Transform moveTo)
    {
        while (!IsRotatedAtPointVersion2(moveFrom, moveTo))
        {
            RotateTowardsVersion2(moveFrom, moveTo);
            await Task.Yield();
        }

        controlledTank.StopTracks();

        while (!IsOnSpot(moveFrom, moveTo))
        {
            MoveForward(moveFrom, moveTo);
            await Task.Yield();
        }

        controlledTank.StopTracks();
    }

    public async void FollowThePath()
    {
        Transform moveFrom;
        Transform moveTo;

        for (int i = 0; WaypointToMoveTo(); i++)
        {
            moveFrom = controlledTank.transform;
            moveTo = waypointList[0];

            await RotateAndMove(moveFrom, moveTo);
            waypointList.RemoveAt(0);
        }
    }
    #endregion

    #region Couroutine Aim Workflow
    IEnumerator RotateTurretAndAimAtTarget()
    {
        while (!IsRotatedAtPointVersion2(turret.transform, target.transform))
        {
            RotateTurret();
            yield return null;
        }
        Debug.Log("finished rotating a turret");
    }

    //use ElevateGunOnTarget() only when aiming for the first time
    IEnumerator ElevateGunOnTarget()
    {
        
        while(!GunIsOnTarget())
        {
            if (GetAngleForGun() > gunVerticalAdjustmentUpperBond )
            {
                ElevateGun(true);
            }
            else
            {
                ElevateGun(false);
            }
            yield return null;
        }
        
    }

    IEnumerator ShootAtTargetAndMakeAdjustments()
    {
        awaitingExplosionCoordinates = false;
        controlledTank.FireMainGun();
        objectThatWasHit = null;
        awaitingExplosionCoordinates = true;        
        
        //add timer to await reloading a gun;
        while (awaitingExplosionCoordinates)
        {
            yield return null;
        }

        if (WasTheTargetHit())
        {
            Debug.Log("I've hit the target");
            yield break;

        }
        
        
        Debug.Log("received coordinates making adjustment");

        float angleBeforeAdjustment = GetAngleForGun();
        float angleAfterAdjustment = GetGunAdjustmentValue(angleBeforeAdjustment);

        awaitingGunAdjustment = true;

        StartCoroutine(ApplyGunAdjustment(angleBeforeAdjustment, angleAfterAdjustment));

        while (awaitingGunAdjustment)
        {
            yield return null;
        }

        Debug.Log("Finished applying gun adjustments");

    }

    IEnumerator ApplyGunAdjustment(float angleBeforeAdjustment, float angleAfterAdjustment) 
    {
        Debug.Log("Starting ApplyGunAdjustment");


        if (angleBeforeAdjustment > angleAfterAdjustment)
        {
            Debug.Log("Adjusting gun UP");
            while (GetAngleForGun() > angleAfterAdjustment)
            {
                
                ElevateGun(true);
                //if you do not add the YIELD RETURN NULL; it will move the Gun instantly;
                yield return null;
                
            }
            Debug.Log("Finished adjusting gun UP");
            awaitingGunAdjustment = false;
            yield break;
            
        }

        if (angleBeforeAdjustment < angleAfterAdjustment)
        {
            Debug.Log("Adjusting gun down");

            while (GetAngleForGun() < angleAfterAdjustment)
            {
                ElevateGun(false);
                //if you do not add the YIELD RETURN NULL; it will move the Gun instantly;
                yield return null;
            }
            Debug.Log("Finished adjusting gun down");
            awaitingGunAdjustment = false;
            yield break;
        }

        


       
    }
    float GetGunAdjustmentValue(float angleBeforeAdjustment)
    {
        float angleAfterAdjustment;
        if (TooHigh())
        {
            angleAfterAdjustment = angleBeforeAdjustment + 1f;
        }

        else
        {
            angleAfterAdjustment = angleBeforeAdjustment - 1f;
        }

        return angleAfterAdjustment;
    }


    bool WasTheTargetHit()
    {
        if (objectThatWasHit == target)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    bool TooHigh()
    {
        float distanceToExplosion = Vector3.Distance(tankToMove.transform.position, hitCoordinates);
        float distanceToTarget = Vector3.Distance(tankToMove.transform.position, target.transform.position);
        
        if (distanceToExplosion > distanceToTarget)
        {
            Debug.Log("gun is too high");
            return true;
        }

        else
        {
            Debug.Log("gun is too low");
            return false;
        }
    }

    #endregion
    bool WaypointToMoveTo() //checks if the waypoint list is empty or not
    {
        if (waypointList.Count == 0)
        {
            Debug.Log("ти приїхав, хлопче");
            return false;
        }

        return true;

    }


    

}
