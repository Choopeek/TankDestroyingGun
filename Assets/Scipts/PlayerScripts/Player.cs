using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerVehicle;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HandlePlayerScript();
    }

    void HandlePlayerScript()
    {
        if (playerVehicle != null)
        {
            gameManager.playerVehicle = playerVehicle;           
            return;
        }

        if (playerVehicle == null)
        {
            StartCoroutine(WaitAndSetPlayerVehicle());
        }
    }

    IEnumerator WaitAndSetPlayerVehicle()
    {
        yield return new WaitForSeconds(1);
        HandlePlayerScript();
        yield return null;
    }
}
