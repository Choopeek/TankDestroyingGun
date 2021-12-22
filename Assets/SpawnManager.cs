using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] List<GameObject> spawnPrefabList;
    int spawnPrefabListLenght;

    [SerializeField] List<Transform> spawnPoint;

    public int waveNumber;
    


    private void Start()
    {
        waveNumber = 0;
        spawnPrefabListLenght = spawnPrefabList.Capacity;
        SpawnWave();
    }

    int SelectObjectToSpawn()
    {
        int objectToSpawn = Random.Range(0, spawnPrefabListLenght);
        return objectToSpawn;
    }

    void SpawnWave ()
    {
        for (int i = 0; i < spawnPrefabListLenght; i++)
        {
            int objectToSpawn = SelectObjectToSpawn();
            Instantiate(spawnPrefabList[objectToSpawn], spawnPoint[i].transform.position, spawnPrefabList[objectToSpawn].transform.rotation);
        }
        waveNumber++;
    }
}
