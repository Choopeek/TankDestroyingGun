using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDistanceScript : MonoBehaviour
{

    [SerializeField] public Transform target;

    [SerializeField] public Vector3 targetOldCoordinates;
    [SerializeField] public Vector3 targetNewCoordinates;


    void DistanceCheck()
    {
        targetNewCoordinates = target.position;
        Debug.Log("Old coordinates are " + targetOldCoordinates);
        Debug.Log("New coordinates are " + targetNewCoordinates);
        float distance = Vector3.Distance(targetOldCoordinates, targetNewCoordinates);
        Debug.Log("You moved " + distance + " meters");



    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            targetOldCoordinates = target.position;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            DistanceCheck();
        }
    }
}
