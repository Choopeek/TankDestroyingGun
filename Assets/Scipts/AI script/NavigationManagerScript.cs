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

    



    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (tankToMove == null)
            {
                return;
            }
            controlledTankNavAgent.waypointList = new List<Transform>(waypointList);
            StartCoroutine(controlledTankNavAgent.FollowThePathCor());
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            GetScript();
            Debug.Log("Button L pressed");
        }
    }

}
