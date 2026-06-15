using UnityEngine;
using System.Collections.Generic;

public class SaveEnemy : MonoBehaviour
{
    [SerializeField]GameObject enemyPrefab;
    private List<GameObject> enemies;

    public void SaveEnemies()
    {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        List<EnemyData> enemyPositions = new List<EnemyData>();
        foreach (GameObject enemy in enemies)
        {
            enemyPositions.Add(new EnemyData { position = new List<float> { enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z } });
        }
        SaveSystem.instance.SaveSection("enemies", enemyPositions);
    }
    public void LoadEnemies()
    {
        List<EnemyData> enemyPositions = SaveSystem.instance.LoadSection<List<EnemyData>>("enemies");
        if (enemyPositions != null)
        {
            foreach (EnemyData enemyData in enemyPositions)
            {
                Vector3 position = new Vector3(enemyData.position[0], enemyData.position[1], enemyData.position[2]);
                Instantiate(enemyPrefab, position, Quaternion.identity);
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveEnemies();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadEnemies();
        }
    }



}

class EnemyData
{
    public List<float> position;
}
