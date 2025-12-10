using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Shooter.Gameplay
{
    public class EnemyGroup : MonoBehaviour
    {
        [HideInInspector]
        public GameObject[] m_Enemies;

        public EnemySpawnPoint[] m_EnemyPoints;

        [HideInInspector]
        public int m_EnemyCount = 0;

        [HideInInspector]
        public bool KilledAll = false;

        public UnityEvent OnKilledAll;
        // Start is called before the first frame update
        void Start()
        {
            KilledAll = false;
            m_EnemyCount = m_EnemyPoints.Length;
            //Spawn();
        }

        // Update is called once per frame
        void Update()
        {
            if (!KilledAll)
            {
                if (m_EnemyCount <=0)
                {
                    KilledAll = true;
                    OnKilledAll.Invoke();
                }
            }
        }

        public void Spawn()
        {
            KilledAll = false;
            m_EnemyCount = m_EnemyPoints.Length;

            foreach (EnemySpawnPoint point in m_EnemyPoints)
            {
                //point.SpawnEnemy(this);
            }
        }

        public void DecreaceEnemyCount()
        {
            m_EnemyCount--;
        }
    }
}
