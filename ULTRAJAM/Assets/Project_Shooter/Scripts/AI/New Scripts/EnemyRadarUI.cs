using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Gameplay
{
    public class EnemyRadarUI : MonoBehaviour
    {
        [SerializeField] private GameObject indicatorPrefab;
        [SerializeField] private int maxIndicators = 20;
        [SerializeField] private float indicatorMargin = 50f; 

        private List<EnemyIndicator> indicatorPool = new();
        private Dictionary<GameObject, EnemyIndicator> activeIndicators = new ();
        private SpawnControl spawnControl;

        void Awake()
        {
            InitializePool();
            spawnControl = FindAnyObjectByType<SpawnControl>();

            if (spawnControl != null)
            {
                spawnControl.OnEnemySpawned.AddListener(OnEnemySpawned);
                spawnControl.OnEnemyDestroyed.AddListener(OnEnemyDestroyed);
            }
        }


        public void Update()
        {
            if (spawnControl is null || spawnControl.enemies is null)
                return;

            var enemiesToRemove = new List<GameObject>();

            foreach (var kvp in activeIndicators)
                if (kvp.Key is null || !spawnControl.enemies.Contains(kvp.Key))
                    enemiesToRemove.Add(kvp.Key);

            foreach (var enemy in enemiesToRemove)
                ReturnIndicatorToPool(enemy);

            foreach (var enemy in spawnControl.enemies)
            {
                if (enemy is null) continue;

                if (!activeIndicators.ContainsKey(enemy))
                {
                    // Создаем новый индикатор
                    var indicator = GetIndicatorFromPool();
                    if (indicator != null)
                    {
                        indicator.SetTarget(enemy.transform);
                        indicator.SetMargin(indicatorMargin);
                        indicator.gameObject.SetActive(true);
                        activeIndicators[enemy] = indicator;
                    }
                    else
                    {
                        activeIndicators[enemy].SetMargin(indicatorMargin);
                    }
                }
            }
        }

        private void InitializePool()
        {
            for (int i = 0; i < maxIndicators; i++)
            {
                GameObject indicatorObj = Instantiate(indicatorPrefab, transform);
                EnemyIndicator indicator = indicatorObj.GetComponent<EnemyIndicator>();
                indicator.SetMargin(indicatorMargin);
                indicatorObj.SetActive(false);
                indicatorPool.Add(indicator);
            }
        }

        private EnemyIndicator GetIndicatorFromPool()
        {
            foreach (var indicator in indicatorPool)
                if (!indicator.gameObject.activeInHierarchy)
                    return indicator;

            // Если все индикаторы заняты, создаем новый
            if (indicatorPool.Count < maxIndicators * 2) // Удваиваем лимит при необходимости
            {
                var indicatorObj = Instantiate(indicatorPrefab, transform);
                var indicator = indicatorObj.GetComponent<EnemyIndicator>();
                indicator.SetMargin(indicatorMargin);
                indicatorPool.Add(indicator);
                return indicator;
            }

            return null;
        }

        private void ReturnIndicatorToPool(GameObject enemy)
        {
            if (activeIndicators.TryGetValue(enemy, out EnemyIndicator indicator))
            {
                indicator.gameObject.SetActive(false);
                activeIndicators.Remove(enemy);
            }
        }

        // Альтернативный метод для отслеживания через события
        public void OnEnemySpawned(GameObject enemy)
        {
            EnemyIndicator indicator = GetIndicatorFromPool();
            if (indicator != null)
            {
                indicator.SetTarget(enemy.transform);
                indicator.gameObject.SetActive(true);
                activeIndicators[enemy] = indicator;
            }
        }

        public void OnEnemyDestroyed(GameObject enemy)
        {
            ReturnIndicatorToPool(enemy);
        }
    }
}