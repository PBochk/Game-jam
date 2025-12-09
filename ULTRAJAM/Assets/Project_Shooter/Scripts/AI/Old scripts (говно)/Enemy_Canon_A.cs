using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Enemy_Canon_A : Enemy
    {
        public GameObject m_BulletPrefab1;
        public GameObject m_FireParticlePrefab1;
        public GameObject m_PreFireParticlePrefab1;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_AttackLoop());
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(transform.position, PlayerChar.m_Current.transform.position) <= 30f)
            {
                m_FacePlayer = true;
            }
            else
            {
                m_FacePlayer = false;
            }
            //StateSystem stateSys = GetComponent<StateSystem>();
            //switch (stateSys.CurrentState)
            //{
            //    case 0:
            //        if (Vector3.Distance(transform.position, PlayerChar.m_Current.transform.position) <= 30f)
            //        {
            //            stateSys.StartState(1);
            //            m_FacePlayer = true;
            //        }
                        
            //        break;

            //    case 1:
            //        if (stateSys.StateTimer > 1f)
            //        {
            //            stateSys.StartState(2);
            //        }
            //        break;

            //    case 2:
            //        ShootBullet();
            //        stateSys.StartState(3);
            //        break;

            //    case 3:
            //        if (stateSys.StateTimer > 1f)
            //        {
            //            stateSys.StartState(1);
            //        }
            //        break;
            //}

            HandleFacePlayer();
            HandleDeath();
        }

        IEnumerator Co_AttackLoop()
        {
           
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                if (Vector3.Distance(transform.position, PlayerChar.m_Current.transform.position) <= 30f)
                {
                    GameObject obj = Instantiate(m_PreFireParticlePrefab1);
                    obj.transform.SetParent(m_FirePoint);
                    obj.transform.localPosition = Vector3.zero;
                    Destroy(obj, 3);

                    yield return new WaitForSeconds(1.3f);
                    ShootBullet();
                }
                yield return new WaitForSeconds(1.5f);

            }
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
    }
}
