using UnityEngine;

/*
    hi guys, jj hier 
    dit is de script voor enemy spawning (sorry als de code slechte is)
    geef gerust feedback :D
*/
public class EnemySpawning : MonoBehaviour
{
    [Header ("Enemy Prefab")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int randomEnemyCount = 3;

    [Header("Spawning Settings (for set area)")]
    [SerializeField] private Transform[ ] spawnPoints;

    [Header("Random Spawning Settings")]
    [SerializeField] private bool useRandomArea = true;
    [SerializeField] private float spawnAreaRadius = 5f;

    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float triggerRadius = 10f;

    private bool hasSpawned = false;
    void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, spawnAreaRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, triggerRadius);

    }
    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        if (hasSpawned) return;

        // is voor debugging

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemies();
            hasSpawned = true;
            return;
        }

        if (player == null) return;


        if (Vector3.Distance(transform.position, player.position) <= triggerRadius)
        {
            SpawnEnemies();
            hasSpawned = true;
        }
    }

    private void SpawnEnemies()
    {
        if (enemyPrefab == null) return;

        if (!useRandomArea && spawnPoints != null && spawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
        else if (useRandomArea)
        {
            for (int i = 0; i < randomEnemyCount; i++)
            {
                Vector2 randomPoint = Random.insideUnitCircle * spawnAreaRadius;
                Vector3 spawnPosition = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);

                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
