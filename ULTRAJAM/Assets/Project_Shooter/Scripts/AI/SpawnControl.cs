using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Gameplay
{
    public class SpawnControl : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private GameObject player;
        [SerializeField] private int MaxEnemiesCount;
        [SerializeField] private int MinEnemiesCount;
        [SerializeField] private List<SpawnerPoint> spawnPoints;
        public bool isCanSpawn = true;
        public int CurrentEnemiesCount;
        public List<GameObject> enemies;

        public void Awake()
        {
            object value = Random.Range(1, 5);
        }

        public void Update()
        {
            if (!isCanSpawn)
                return;

            if (enemies.Count <= 0)
            {
                StartCoroutine(FillEnemiesList(Random.Range(MinEnemiesCount, MaxEnemiesCount)));
            }
        }

        private IEnumerator FillEnemiesList(int enemiesInWave)
        {
            for (var i = 0; i < enemiesInWave; i++)
            {
                enemies.Add(
                    spawnPoints[Random.Range(0, spawnPoints.Count - 1)]
                    .SpawnEnemy(enemyPrefabs[Random.Range(0, enemyPrefabs.Count - 1)])
                    );
                yield return new WaitForSeconds(2);
            }
        }
        
    }
}
