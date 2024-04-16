using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Settings")]
    public float coolDown;
    public float[] spawnRate;

    [Header("Reference")]
    public List<GameObject> enemyPrefabs;
    public List<Transform> waypointSpawn;

    public void ReduceCoolDown()
    {
        coolDown -= 1f;
    }

    void Start()
    {
        StartCoroutine(SpawnEnemyWithDelay());
    }

    public void SpawnEnemy()
    {
        float totalWeight = 0;
        foreach (float weight in spawnRate)
        {
            totalWeight += weight;
        }

        float randomNum = Random.Range(0, totalWeight);
        float weightSum = 0;
        int selectedIndex = 0;
        for (int i = 0; i < spawnRate.Length; i++)
        {
            weightSum += spawnRate[i];
            if (randomNum <= weightSum)
            {
                selectedIndex = i;
                break;
            }
        }

        int waypointIndex = Random.Range(0, waypointSpawn.Count);
        Instantiate(enemyPrefabs[selectedIndex], waypointSpawn[waypointIndex].position, Quaternion.identity);

        float nextSpawnTime = spawnRate[selectedIndex];
        StartCoroutine(StartNextSpawn(nextSpawnTime));
    }

    IEnumerator SpawnEnemyWithDelay()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(coolDown);
        }
    }

    IEnumerator StartNextSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemy();
    }
}
