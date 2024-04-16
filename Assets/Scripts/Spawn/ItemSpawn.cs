using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [Header("Settings")]
    public float coolDown;

    [Header("Reference")]
    public List<GameObject> itemPrefabs;
    public List<Transform> waypointSpawn;

    private bool isCoolingDown = false;

    void Start()
    {
        SpawnItem();
    }

    void Update()
    {
        if (!isCoolingDown)
        {
            StartCoroutine(SpawnCooldown());
        }
    }

    IEnumerator SpawnCooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(coolDown);
        SpawnItem();
        isCoolingDown = false;
    }

    void SpawnItem()
    {
        if (itemPrefabs.Count == 0 || waypointSpawn.Count == 0)
        {
            Debug.LogWarning("ItemPrefabs or WaypointSpawn is empty. Cannot spawn items.");
            return;
        }

        GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        Transform randomWaypoint = waypointSpawn[Random.Range(0, waypointSpawn.Count)];
        Instantiate(randomItemPrefab, randomWaypoint.position, randomWaypoint.rotation);
    }
}
