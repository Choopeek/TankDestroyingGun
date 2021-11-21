using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTankController : MonoBehaviour
{
    Tank controlledTank;
    [SerializeField] public GameObject playerTank;
    
    void Start()
    {
        controlledTank = playerTank.GetComponent<Tank>();
        
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

    void CODESAVER()
    {
        //just put here all the code you don't want to delete right now and its too long to comment all lines.
    }
    
}
