using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerTankController player;
    [SerializeField] CinemachineFreeLook cinemachineCam;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerTankController>();
        cinemachineCam = GetComponent<CinemachineFreeLook>();
        cinemachineCam.Follow = player.playerTank.transform;
        cinemachineCam.LookAt = player.playerTank.transform;

    }

    

    
    
}
