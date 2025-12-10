using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static Shooter.Gameplay.SpawnControl;

namespace Shooter.Gameplay
{

    public class SpawnControl : MonoBehaviour
    {
        



        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private GameObject player;
        [SerializeField] private int MaxEnemiesCount = 15;
        [SerializeField] private int MinEnemiesCount = 5;
        [SerializeField] private int NeedCountOfKills = 40;
        [SerializeField] private List<SpawnerPoint> spawnPoints;

        [SerializeField] private int[] stageKillThresholds = new int[] { 0, 10, 25 }; // Пороги убийств для стадий
        [SerializeField] private int[] stageMinEnemies = new int[] { 5, 8, 12 }; // Мин врагов на стадии
        [SerializeField] private int[] stageMaxEnemies = new int[] { 10, 15, 20 };

        public int CountOfKills =  0;
        public bool isCanSpawn = true;
        public int CurrentEnemiesCount;
        public List<GameObject> enemies;
        private bool isSpawningWave = false;
        public UnityEvent<GameObject> OnEnemySpawned;
        public UnityEvent<GameObject> OnEnemyDestroyed;
        public UnityEvent<int> OnStageChanged;


        private int enemyPrefabThatCanSpawn = 0;


        private int currentStage = 0;
        private int maxPrefabIndex = 0;


        public void Update()
        {

            if (CountOfKills == NeedCountOfKills)
                SceneManager.LoadScene("CompilationScene");
            enemies.RemoveAll(enemy => enemy is null);
            CurrentEnemiesCount = enemies.Count;


            if (!isCanSpawn || isSpawningWave)
                return;

            CheckStageUpdate();

            if (enemies.Count <= 0)
            {
                StartCoroutine(FillEnemiesList());
            }
        }

        private void CheckStageUpdate()
        {
            int newStage = 0;

            for (int i = stageKillThresholds.Length - 1; i >= 0; i--)
            {
                if (CountOfKills >= stageKillThresholds[i])
                {
                    newStage = i;
                    break;
                }
            }

            if (newStage != currentStage)
            {
                currentStage = newStage;
                maxPrefabIndex = Mathf.Min(currentStage, enemyPrefabs.Count - 1);


                Debug.Log($"Переход на стадию {currentStage}! " +
                         $"Врагов: {stageMinEnemies[currentStage]}-{stageMaxEnemies[currentStage]}, " +
                         $"Макс. префаб: {maxPrefabIndex}");

                OnStageChanged?.Invoke(currentStage);
            }
        }

       

        private IEnumerator FillEnemiesList()
        {
            isSpawningWave  = true;

            var minEnemies = stageMinEnemies[currentStage];
            var maxEnemies = stageMaxEnemies[currentStage];

            var enemiesToSpawn = Random.Range(minEnemies, maxEnemies + 1);

            var enemiesPerPoint = new Dictionary<SpawnerPoint, int>();
    


            for (var i = 0; i < enemiesToSpawn; i++)
            {
                var point = spawnPoints[i % spawnPoints.Count];
                if (!enemiesPerPoint.ContainsKey(point))
                    enemiesPerPoint[point] = 0;
                enemiesPerPoint[point]++;
            }

            var coroutines = new List<Coroutine>();
            foreach (var kvp in enemiesPerPoint)
            {
                var coroutine = StartCoroutine(SpawnEnemiesInPointCoroutine(kvp.Key, kvp.Value));
                coroutines.Add(coroutine);
            }

            foreach (var coroutine in coroutines)
            {
                yield return coroutine;
            }

            isSpawningWave = false;

        }

        private IEnumerator SpawnEnemiesInPointCoroutine(SpawnerPoint spawnPoint, int enemiesCount)
        {
            for (var i = 0; i < enemiesCount; i++)
            {

                if (i == 0)
                {
                    var initialDelay = Random.Range(0f, 5f); // Первые враги появляются не одновременно
                    yield return new WaitForSeconds(initialDelay);
                }
                else
                {
                    var delayBetweenEnemies = Random.Range(1f, 10f);
                    yield return new WaitForSeconds(delayBetweenEnemies);
                }

                int prefabIndex = GetEnemyPrefabIndexForStage();
                var enemyPrefab = enemyPrefabs[prefabIndex];


                var spawnedEnemy = spawnPoint.SpawnEnemy(enemyPrefab);



                enemies.Add(spawnedEnemy);
                var tracker = spawnedEnemy.AddComponent<EnemyTracker>();
                tracker.OnEnemyDestroyed += () => RemoveEnemy(spawnedEnemy);

                OnEnemySpawned?.Invoke(spawnedEnemy);
            }
        }
        private int GetEnemyPrefabIndexForStage()
        {
            // На стадии 0: только враг 0
            // На стадии 1: враги 0-1 с большим шансом на врага 1
            // На стадии 2: враги 0-2 с большим шансом на врага 2

            if (currentStage == 0)
                return 0; // Только первый враг
            else if (currentStage == 1)
                // 70% шанс на врага 1, 30% на врага 0
                return Random.value < 0.7f ? 1 : 0;
            else // currentStage == 2
            {
                // 60% шанс на врага 2, 30% на врага 1, 10% на врага 0
                var rand = Random.value;
                if (rand < 0.6f) return 2;
                if (rand < 0.9f) return 1;
                return 0;
            }
        }

        public void SetStage(int stage)
        {
            if (stage >= 0 && stage < stageKillThresholds.Length)
            {
                currentStage = stage;
                maxPrefabIndex = Mathf.Min(currentStage, enemyPrefabs.Count - 1);
                OnStageChanged?.Invoke(currentStage);

                Debug.Log($"Принудительно установлена стадия {currentStage}");
            }
        }
        public int GetCurrentStage()
        {
            return currentStage;
        }
        private void RemoveEnemy(GameObject enemy)
        {
            CountOfKills++;
            if (enemies.Contains(enemy)) 
                enemies.Remove(enemy);

            OnEnemyDestroyed?.Invoke(enemy);
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
