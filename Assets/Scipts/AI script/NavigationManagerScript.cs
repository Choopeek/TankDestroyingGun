using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NavigationManagerScript : MonoBehaviour
{
    [SerializeField] public GameObject tankToMove;
    NavigationManagerAgent controlledTankNavAgent;
    
    
    [SerializeField] List<Transform> waypointList = new List<Transform>();

    

    void GetScript()
    {
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
        }
        
        
        
    }


    //before creating new tankToMove or anything else - trigger method SendWaypointList(), and after it StartMovement(); So the tank will start moving;
    void SendWaypointList()
    {
        controlledTankNavAgent.waypointList = new List<Transform>(waypointList);
    }

    void StartMovement()
    {
        StartCoroutine(controlledTankNavAgent.FollowThePathCor());
    }

    



    

}
