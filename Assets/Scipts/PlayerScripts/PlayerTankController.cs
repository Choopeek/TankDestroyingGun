using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;

public class PlayerTankController : MonoBehaviour
{
    Tank controlledTank;
    [SerializeField] public GameObject playerTank;

    [SerializeField] Vector3 tankPrecisionCamera;
    [SerializeField] CinemachineVirtualCameraBase thirdPersonCam;
    [SerializeField] CinemachineVirtualCameraBase aimCamera;
    bool aimCamEnabled = false;

    [SerializeField] GameObject aimCamPoint;


    // HUD variables
    [SerializeField] Canvas tankHUDCanvas;
    [SerializeField] CanvasGroup tankHUDCanvasGroup;
    [SerializeField] GameObject tankCrosshair;
    [SerializeField] GameObject hudTankTurretTurnMarker;
    [SerializeField] GameObject hudTankChassisTurnMarker;

    [SerializeField] int chanceOfHappening;
     
    
    
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
        #region tankControls
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
        #endregion

        AllignGunAndAimCamera();


        #region TestMethodsControls

        if (Input.GetKeyDown(KeyCode.P))
        {
            //Debug.Log("Test P pressed");
            
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            //Debug.Log("Test O pressed");
            
        }
        #endregion



    }







    IEnumerator MakeHUDTransparent(bool makeHUDtransparent)
    {
        if (makeHUDtransparent)
        {
            while (tankHUDCanvasGroup.alpha > 0)
            {
                float value = tankHUDCanvasGroup.alpha;
                tankHUDCanvasGroup.alpha = Mathf.Lerp(0, 1, 0);
                yield return null;
            }
        }

        if (!makeHUDtransparent)
        {
            while (tankHUDCanvasGroup.alpha < 1)
            {
                float value = tankHUDCanvasGroup.alpha;
                tankHUDCanvasGroup.alpha = Mathf.Lerp(0, 1, value + Time.deltaTime);
                yield return null;
            }
        }

        
        

    }


    void EnableDisableHUD(bool enable)
    {
        if (enable)
        {
            tankCrosshair.gameObject.SetActive(true);
            hudTankChassisTurnMarker.gameObject.SetActive(true);
            hudTankTurretTurnMarker.gameObject.SetActive(true);
            StartCoroutine(MakeHUDTransparent(false));

        }
        else
        {
            StartCoroutine(MakeHUDTransparent(true));
            tankCrosshair.gameObject.SetActive(false);
            hudTankChassisTurnMarker.gameObject.SetActive(false);
            hudTankTurretTurnMarker.gameObject.SetActive(false);
        }
        
    }
    private void AllignGunAndAimCamera()
    {
        //making sure that crosshair alligns withgun
        aimCamera.transform.rotation = controlledTank.gun.transform.rotation;
        //show player where his turret is rotated
        hudTankTurretTurnMarker.transform.localEulerAngles = new Vector3(0, 0, -controlledTank.turret.gameObject.transform.localEulerAngles.y);
    }

    
    void SwitchCamera()
    {
        if (!aimCamEnabled)
        {
            aimCamEnabled = true;
            thirdPersonCam.Priority = 1;
            aimCamera.Priority = 2;
            EnableDisableHUD(true);
            Debug.Log("Switch to AIM");
        }
        else
        {
            aimCamEnabled = false;
            thirdPersonCam.Priority = 2;
            aimCamera.Priority = 1;
            EnableDisableHUD(false);
            Debug.Log("Switch to 3d Person");
        }
    }
    void CODESAVER()
    {
        //just put here all the code you don't want to delete right now and its too long to comment all lines.
    }
    
}
