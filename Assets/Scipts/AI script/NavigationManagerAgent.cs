using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NavigationManagerAgent : MonoBehaviour
{
    public List<Transform> waypointList = new List<Transform>();

    public GameObject tankToMove;
    Tank controlledTank;

    public void GetScript()
    {
        if (tankToMove == null)
        {
            Debug.Log("NoTankToControll");
            return;
        }
        controlledTank = tankToMove.GetComponent<Tank>();        
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



    void RotateTowards(Transform from, Transform to)
    {




        if (!IsRotatedAtPoint(from, to))
        {
            float speed = 1 * Time.deltaTime;
            Vector3 targetPos = to.position - from.position;
            Vector3 newRotation = Vector3.RotateTowards(tankToMove.transform.forward, targetPos, speed, 0f);
            tankToMove.transform.rotation = Quaternion.LookRotation(newRotation);
        }
        else
        {

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

        if (angle >= 89 && angle <= 91)
        {
            //isRotated
            Debug.Log("IsRotated");
        }

    }



    bool IsRotatedAtPoint(Transform from, Transform to)
    {
        float rotationDifference = GetRotationDifference(from, to);

        if (rotationDifference > 0.999f)
        {

            return true;
        }
        else
        {
            return false;
        }

    }

    bool IsRotatedAtPointVersion2(Transform angleFrom, Transform angleTo)
    {
        float angle = GetAngleRight(angleFrom, angleTo);
        if (angle >= 89 && angle <= 91)
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



    

    #region Couroutine Workflow
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
            Destroy(this);
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

    #region Task Workflow

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
