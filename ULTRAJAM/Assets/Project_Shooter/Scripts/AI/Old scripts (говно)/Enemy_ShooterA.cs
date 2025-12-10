using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Enemy_ShooterA : Enemy
    {
        public GameObject m_BulletPrefab1;
        public GameObject m_FireParticlePrefab1;
        bool m_Spawned = false;
        // Start is called before the first frame update
        void Start()
        {
            //StateSystem stateSys = GetComponent<StateSystem>();
            //stateSys.StartState(-2);
            //StartCoroutine(Co_EnterLevel());
            m_FacePlayer = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!m_Spawned)
            {
                if (CameraControl.m_Current.m_CameraTopPosition.z > transform.position.z && transform.position.z >= CameraControl.m_Current.m_CameraBottomPosition.z)
                {
                    m_Spawned = true;
                    StartCoroutine(Co_EnterLevel());
                }
            }

            HandleFacePlayer();
            HandleDeath();
        }

        IEnumerator Co_EnterLevel()
        {
            EnemyMovement movement = GetComponent<EnemyMovement>();
            movement.m_UsePhysics = false;
            movement.SetMoveTargetPosition(transform.position + m_StartWalkDistance * transform.forward);
            m_FacePlayer = false;
            while (!movement.m_ReachedTargetPosition)
            {
                yield return null;
            }
            //StateSystem stateSys = GetComponent<StateSystem>();
            //stateSys.StartState(0);
            StartCoroutine(Co_AttackLoop());
            //yield return null;
        }


        public void ShootBullet()
        {
            GameObject obj = Instantiate(m_BulletPrefab1);
            obj.transform.position = m_FirePoint.position;
            Vector3 dir = PlayerChar.m_Current.transform.position - transform.position;
            dir.y = 0;
            obj.transform.forward = dir;
            obj.GetComponent<ProjectileCollision>().m_Creator = gameObject;
            Destroy(obj, 10);

            obj = Instantiate(m_FireParticlePrefab1);
            obj.transform.position = m_FirePoint.position;
            Destroy(obj, 3);
        }

        IEnumerator Co_AttackLoop()
        {
            m_FacePlayer = true;
            while (true)
            {
                yield return new WaitForSeconds(.1f);
                if (Vector3.Distance(transform.position, PlayerChar.m_Current.transform.position) <= 30f)
                {
                    ShootBullet();
                }
                yield return new WaitForSeconds(1.5f);

            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + m_StartWalkDistance * transform.forward);
        }
    }
}
