using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public GameObject itemPrefab;
    public Transform[] spawnPoints;
}

public class ItemSpawner : MonoBehaviour
{
    public SpawnData[] itemsToSpawn;

    void Start()
    {
        foreach (SpawnData data in itemsToSpawn)
        {
            foreach (Transform point in data.spawnPoints)
            {
                Instantiate(
                    data.itemPrefab,
                    point.position,
                    point.rotation
                );
            }
        }
    }
}