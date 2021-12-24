using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] List<GameObject> spawnPrefabList;
    int spawnPrefabListLenght;

    [SerializeField] List<Transform> spawnPoint;

    public int waveNumber;
    


    private void Start()
    {
        waveNumber = 0;
        spawnPrefabListLenght = spawnPrefabList.Count;
        
    }

    int SelectObjectToSpawn()
    {
        int objectToSpawn = Random.Range(0, spawnPrefabListLenght);
        return objectToSpawn;
    }

    public void SpawnWave ()
    {
        for (int i = 0; i < spawnPoint.Count; i++)
        {
            GameObject spawnedEnemy;
            int objectToSpawn = SelectObjectToSpawn();
            spawnedEnemy = Instantiate(spawnPrefabList[objectToSpawn], spawnPoint[i].transform.position, spawnPrefabList[objectToSpawn].transform.rotation) as GameObject;
            gameManager.enemiesList.Add(spawnedEnemy);
            StartCoroutine(gameManager.navigationManagerSCR.LaunchingEnemy(spawnedEnemy, i + 1, gameManager.playerVehicle));
        }
        waveNumber++;
    }
}
