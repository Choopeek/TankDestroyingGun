using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class PlayerTankController : MonoBehaviour
{
    Tank controlledTank;
    [SerializeField] public GameObject playerTank;

    [SerializeField] Vector3 tankPrecisionCamera;
    [SerializeField] CinemachineVirtualCameraBase thirdPersonCam;
    [SerializeField] CinemachineVirtualCameraBase aimCamera;
    bool aimCamEnabled = false;

    [SerializeField] GameObject aimCamPoint;
    
    
    void Start()
    {
        HandlePlayerController();
    }

    void HandlePlayerController()
    {
        controlledTank = playerTank.GetComponent<Tank>();
        aimCamPoint = playerTank.transform.GetChild(2).GetChild(1).gameObject;
        aimCamera.transform.parent = aimCamPoint.transform;
        aimCamera.transform.position = aimCamPoint.transform.position;
    }
    void Update()
    {
        
        //basic tank controls
        if (Input.GetKey(KeyCode.W))
        {
            controlledTank.MoveTankForward();
        }

        if (Input.GetKey(KeyCode.S))
        {
            controlledTank.MoveTankBackwards();
        }

        if (Input.GetKey(KeyCode.A))
        {
            controlledTank.RotateTankLeft(true);
        }

        if (Input.GetKey(KeyCode.D))
        {
            controlledTank.RotateTankLeft(false);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            controlledTank.RotateTurretLeft(true);
            
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            controlledTank.RotateTurretLeft(false);
            
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            controlledTank.MoveGunUp(true);            
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            controlledTank.MoveGunUp(false);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SwitchCamera();
        }

        //if player releases the control button\s. tracks stop moving
        if (Input.GetKeyUp(KeyCode.W) | Input.GetKeyUp(KeyCode.S) | Input.GetKeyUp(KeyCode.A) | Input.GetKeyUp(KeyCode.D))
        {
            controlledTank.StopTracks();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            controlledTank.FireMainGun();
        }



    }

    void SwitchCamera()
    {
        if (!aimCamEnabled)
        {
            aimCamEnabled = true;
            thirdPersonCam.Priority = 1;
            aimCamera.Priority = 2;
            Debug.Log("Switch to AIM");
        }
        else
        {
            aimCamEnabled = false;
            thirdPersonCam.Priority = 2;
            aimCamera.Priority = 1;
            Debug.Log("Switch to 3d Person");
        }
    }
    void CODESAVER()
    {
        //just put here all the code you don't want to delete right now and its too long to comment all lines.
    }
    
}
