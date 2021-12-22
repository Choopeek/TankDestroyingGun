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
    [SerializeField] CinemachineVirtualCamera aimCameraControls;
    bool aimCamEnabled = false;
    [SerializeField] Camera mainCamera;

    [SerializeField] GameObject aimCamPoint;


    // HUD variables
    [SerializeField] Canvas tankHUDCanvas;
    [SerializeField] CanvasGroup tankHUDCanvasGroup;
    [SerializeField] GameObject tankCrosshair;
    [SerializeField] GameObject hudTankTurretTurnMarker;
    [SerializeField] GameObject hudTankChassisTurnMarker;
    bool handlingHUD;
    int zoomValue = 0;
    int zoomCameraRotationAdjustValue = 0;

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

        if (Input.GetKeyUp(KeyCode.LeftShift) & !handlingHUD)
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
            Debug.Log("Test P pressed from Player Tank Controller");
            
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Test O pressed from Player Tank Controller");

        }
        #endregion



    }







    IEnumerator MakeHUDTransparent(bool makeHUDtransparent)
    {
        handlingHUD = true;
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

        handlingHUD = false;
        

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
        if (!enable)
        {
            StartCoroutine(MakeHUDTransparent(true));
            tankCrosshair.gameObject.SetActive(false);
            hudTankChassisTurnMarker.gameObject.SetActive(false);
            hudTankTurretTurnMarker.gameObject.SetActive(false);
        }
        
    }
    private void AllignGunAndAimCamera()
    {
        //making sure that crosshair alligns with gun
        //aimCamera.transform.localEulerAngles = controlledTank.gun.transform.localEulerAngles;
        aimCamera.transform.localEulerAngles = new Vector3(controlledTank.gun.transform.localEulerAngles.x + zoomCameraRotationAdjustValue, controlledTank.gun.transform.localEulerAngles.y, controlledTank.gun.transform.localEulerAngles.z);
        //show player where his turret is rotated
        hudTankTurretTurnMarker.transform.localEulerAngles = new Vector3(0, 0, -controlledTank.turret.gameObject.transform.localEulerAngles.y);
    }

    
    void SwitchCamera()
    {
        if (!aimCamEnabled)
        {
            
            thirdPersonCam.Priority = 1;
            aimCamera.Priority = 2;
            EnableDisableHUD(true);
            if (zoomValue == 0)
            {
                aimCameraControls.m_Lens.FieldOfView = 40;
                zoomCameraRotationAdjustValue = 0;                
                zoomValue = 1;
                return;
            }

            if (zoomValue == 1)
            {
                aimCameraControls.m_Lens.FieldOfView = 20;
                zoomCameraRotationAdjustValue = 2;                
                zoomValue = 2;
                return;
            }

            if (zoomValue == 2)
            {
                aimCameraControls.m_Lens.FieldOfView = 10;
                zoomCameraRotationAdjustValue = 3;                
                zoomValue = 0;
                aimCamEnabled = true;
            }

            Debug.Log("Switch to AIM");
        }
        else
        {
            aimCamEnabled = false;
            thirdPersonCam.Priority = 2;
            aimCamera.Priority = 1;
            zoomValue = 0;
            EnableDisableHUD(false);
            Debug.Log("Switch to 3d Person");
        }
    }

    
    void CODESAVER()
    {
        //just put here all the code you don't want to delete right now and its too long to comment all lines.
    }
    
}
