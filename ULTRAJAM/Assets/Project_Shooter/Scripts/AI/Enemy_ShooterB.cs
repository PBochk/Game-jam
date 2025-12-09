using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Enemy_ShooterB : Enemy
{
        public GameObject m_BulletPrefab1;
        public GameObject m_FireParticlePrefab1;

        public float m_WalkDistance = 3;
        // Start is called before the first frame update
        void Start()
        {
            InitPosition = transform.position;
            
            
        }

        // Update is called once per frame
        void Update()
        {
            CheckAlert();

            HandleFacePlayer();
            HandleDeath();
        }

        public override void StartAlert()
        {
            base.StartAlert();

            StartCoroutine(Co_EnterLevel());
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

            yield return new WaitForSeconds(1f);
            StartCoroutine(Co_AttackLoop());
           
        }

        IEnumerator Co_AttackLoop()
        {
            EnemyMovement movement = GetComponent<EnemyMovement>();
            StateSystem stateSys = GetComponent<StateSystem>();
            //movement.m_UsePhysics = false;

            Vector3[] points = new Vector3[2];
            points[0] = transform.position + new Vector3(-m_WalkDistance, 0, 0);
            points[1] = transform.position + new Vector3(m_WalkDistance, 0, 0);
            int pointNum = 0;
            while(true)
            {
                movement.m_FaceMoveDirection = true;
                m_FacePlayer = false;
                movement.SetMoveTargetPosition(points[pointNum]);
                while (!movement.m_ReachedTargetPosition)
                {
                    yield return null;
                }

                if (pointNum == 0)
                    pointNum = 1;
                else
                    pointNum = 0;

                movement.m_FaceMoveDirection = false;
                m_FacePlayer = true;
                yield return new WaitForSeconds(1f);

                m_Animator.SetTrigger("Shoot");
                ShootBullet();

                yield return new WaitForSeconds(1f);

            }
        }
        public void ShootBullet()
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject obj = Instantiate(m_BulletPrefab1);
                obj.transform.position = m_FirePoint.position;
                Vector3 dir = PlayerChar.m_Current.transform.position - transform.position;
                dir.y = 0;
                obj.transform.forward = Quaternion.Euler(0,-5f+i*10,0) *dir;
                obj.GetComponent<ProjectileCollision>().m_Creator = gameObject;
                Destroy(obj, 10);

                obj = Instantiate(m_FireParticlePrefab1);
                obj.transform.position = m_FirePoint.position;
                Destroy(obj, 3);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + m_StartWalkDistance * transform.forward);
            Gizmos.DrawLine(transform.position+m_WalkDistance*transform.right, transform.position - m_WalkDistance * transform.right);
        }
    }
}
