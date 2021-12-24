using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NavigationManagerScript : MonoBehaviour
{
    [SerializeField] public GameObject tankToMove;
    NavigationManagerAgent controlledTankNavAgent;

    [SerializeField] List<Transform> spawn1WP = new List<Transform>();
    [SerializeField] List<Transform> spawn2WP = new List<Transform>();
    [SerializeField] List<Transform> spawn3WP = new List<Transform>();


    [SerializeField] List<Transform> waypointList = new List<Transform>();

    
    public IEnumerator LaunchingEnemy(GameObject spawnedObject, int spawnPointNumber, GameObject targetToAttack)
    {
        yield return new WaitForSeconds(1);
        GetScript(spawnedObject, spawnPointNumber, targetToAttack);
        yield return null;
    }
    public void GetScript(GameObject spawnedObject, int spawnPointNumber, GameObject targetToAttack)
    {
        tankToMove = spawnedObject;
        if (tankToMove == null)
        {
            Debug.Log("NoTankToControll");
            return;
        }
        if (tankToMove.GetComponent<NavigationManagerAgent>() == null)
        {
            tankToMove.AddComponent<NavigationManagerAgent>();
            controlledTankNavAgent = tankToMove.GetComponent<NavigationManagerAgent>();
            controlledTankNavAgent.tankToMove = tankToMove;
            controlledTankNavAgent.GetScript();
            controlledTankNavAgent.target = targetToAttack;
            SendWaypointList(spawnPointNumber);
        }
        
        
        
    }


    //before creating new tankToMove or anything else - trigger method SendWaypointList(), and after it StartMovement(); So the tank will start moving;
    void SendWaypointList(int spawnPointNumber)
    {
        waypointList.Clear();
        GenerateWaypointsToSend(spawnPointNumber);
        controlledTankNavAgent.waypointList = new List<Transform>(waypointList);
        StartMovement();
    }

    void GenerateWaypointsToSend(int spawnPointNumber)
    {
      if (spawnPointNumber == 1)
        {
            waypointList = new List<Transform>(spawn1WP);
            return;
        }
      if (spawnPointNumber == 2)
        {
            waypointList = new List<Transform>(spawn2WP);
            return;
        }
      if (spawnPointNumber == 3)
        {
            waypointList = new List<Transform>(spawn3WP);
            return;
        }


        return;
    }

    void StartMovement()
    {
        StartCoroutine(controlledTankNavAgent.FollowThePathCor());
    }

    



    

}
