using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter.Gameplay
{
    public class SpawnControl : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private GameObject player;
        [SerializeField] private int MaxEnemiesCount;
        [SerializeField] private int MinEnemiesCount;
        [SerializeField] private List<SpawnerPoint> spawnPoints;
        public int CountOfKills =  0;
        public bool isCanSpawn = true;
        public int CurrentEnemiesCount;
        public List<GameObject> enemies;

        public UnityEvent<GameObject> OnEnemySpawned;
        public UnityEvent<GameObject> OnEnemyDestroyed;
        public void Update()
        {
            enemies.RemoveAll(enemy => enemy is null);
            CurrentEnemiesCount = enemies.Count;


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
                var spawnedEnemy = spawnPoints[Random.Range(0, spawnPoints.Count)]
                  .SpawnEnemy(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
                enemies.Add(spawnedEnemy);
                var tracker = spawnedEnemy.AddComponent<EnemyTracker>();
                tracker.OnEnemyDestroyed += () => RemoveEnemy(spawnedEnemy);

                OnEnemySpawned?.Invoke(spawnedEnemy);

                yield return new WaitForSeconds(2);
            }
        }

        private void RemoveEnemy(GameObject enemy)
        {
            CountOfKills++;
            Debug.Log(CountOfKills);
            if (enemies.Contains(enemy)) 
                enemies.Remove(enemy);
        }

        private class EnemyTracker : MonoBehaviour
        {
            public System.Action OnEnemyDestroyed;

            private void OnDestroy()
            {
                OnEnemyDestroyed?.Invoke();
            }
        }

    }
}
