using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    public GameObject playerVehicle;

    [SerializeField] SpawnManager spawnManagerSCR;
    [SerializeField] int maxWaveNumber;

    [SerializeField] public NavigationManagerScript navigationManagerSCR;

    public List<GameObject> enemiesList;

    public int playerScore;



    void Start()
    {
        playerScore = 0;
        spawnManagerSCR.SpawnWave();
    }

    public void ObjectWasDestroyed(GameObject whatWasDestroyed, int scoreValueOfDestroyedObject)
    {
        if (WasThePlayerDestroyed(whatWasDestroyed))
        {
            GameOver();
            return;
        }
        else
        {
            HandleEnemyList(whatWasDestroyed);
            playerScore = playerScore + scoreValueOfDestroyedObject;
        }
    }

    private void HandleEnemyList(GameObject whatWasDestroyed)
    {
        enemiesList.Remove(whatWasDestroyed);
        if (enemiesList.Count == 0)
        {
            Debug.Log("you killed everyone");
            SpawnNewWave();
        }
    }

    void SpawnNewWave()
    {
        if (spawnManagerSCR.waveNumber < maxWaveNumber)
        {
            spawnManagerSCR.SpawnWave();
        }

        else
        {
            Debug.Log("max Wave Number Reached. You won. Congratulations");
        }
    }

    private void GameOver()
    {
        Debug.Log("Game over man, game over");
    }

    bool WasThePlayerDestroyed(GameObject whatWasDestroyed)
    {
        if (whatWasDestroyed == playerVehicle)
        {
            Debug.Log("Player vehicle was destroyed");
            return true;
        }
        else
        {
            Debug.Log("Player vehicle was not destroyed");
            return false;
        }
    }
}
