using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        public GameObject m_EnemyPrefab;
        public int m_SpawnCount;
        //public EnemyWave m_MyWave;
        [HideInInspector]
        public bool m_Spawned = false;

        public float m_PositionsAngle = 0;

        Vector3[] m_Points;

        public float m_StartWalkDistance = 10;

        public bool m_AutoSpawn = true;

        // Start is called before the first frame update
        void Start()
        {
            m_Spawned = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!m_Spawned && m_AutoSpawn)
            {
                if (CameraControl.m_Current.m_CameraTopPosition.z > transform.position.z &&  transform.position.z >= CameraControl.m_Current.m_CameraBottomPosition.z)
                {
                    SpawnEnemy();
                    m_Spawned = true;
                }
            }
        }

        public void SpawnEnemy()
        {
            

            for (int i = 0; i < m_SpawnCount; i++)
            {
                GameObject obj = Instantiate(m_EnemyPrefab);
                obj.transform.position = transform.position+transform.rotation* m_Points[i];
                obj.transform.forward = transform.forward;

                Enemy e = obj.GetComponent<Enemy>();
                e.m_StartWalkDistance = m_StartWalkDistance;
            }
            

            //Enemy e = obj.GetComponent<Enemy>();
            //e.MyGroup = group;
            //e.m_MyPlatform = m_MyPlatform;
            //e.m_MyWave = m_MyWave;
        }

      

        void OnValidate()
        {
            if (m_SpawnCount>0)
            {
                m_Points = new Vector3[m_SpawnCount];

                if (m_SpawnCount == 1)
                {
                    m_Points[0] = Vector3.zero;
                }
                else
                {
                    float dAngle = 360f / (float)m_SpawnCount;
                    float lenght = Mathf.Clamp(2f + .3f * (m_SpawnCount - 2), .6f, 7);


                    for (int i = 0; i < m_SpawnCount; i++)
                    {
                        m_Points[i] = Helper.RotatedLenght(i * dAngle+m_PositionsAngle, lenght);
                    }
                }

              
            }
        }

        void OnDrawGizmos()
        {
            if (m_SpawnCount > 0)
            {
                Gizmos.DrawLine(transform.position, transform.position + 2 * transform.forward);
                for (int i = 0; i < m_SpawnCount; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(transform.position + transform.rotation * m_Points[i], .5f);
                    Gizmos.DrawLine(transform.position + transform.rotation * m_Points[i], transform.position + transform.rotation * m_Points[i] + m_StartWalkDistance * transform.forward);
                }
            }
        }
    }
}
