using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Enm_PillarCanon_1 : Enemy
    {
        [Space]
        public GameObject m_BulletPrefab1;
        public GameObject m_FireParticlePrefab1;

        public float m_InitDelay = 0;
        public float m_FireDelay = 1;

        public Transform m_Base;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_AttackLoop());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_AttackLoop()
        {
            yield return new WaitForSeconds(1);

            while (true)
            {
                for (int i = 0; i <= 10; i++)
                {
                    m_Base.localPosition = new Vector3(0, 0.18f * i, 0);
                    yield return null;
                }

                yield return new WaitForSeconds(1);
                ShootRingBullet();
                yield return new WaitForSeconds(1);

                for (int i = 0; i <= 10; i++)
                {
                    m_Base.localPosition = new Vector3(0,1.8f- 0.18f * i, 0);
                    yield return null;
                }

                yield return new WaitForSeconds(2);
            }
        }

        public void ShootBullet()
        {

            GameObject obj = Instantiate(m_BulletPrefab1);
            obj.transform.position = m_FirePoint.position;

            obj.transform.forward = transform.forward;
            obj.GetComponent<ProjectileCollision>().m_Creator = gameObject;
            Destroy(obj, 10);

            obj = Instantiate(m_FireParticlePrefab1);
            obj.transform.position = m_FirePoint.position;
            Destroy(obj, 3);
        }

        public void ShootRingBullet()
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject obj = Instantiate(m_BulletPrefab1);
                obj.transform.position = m_FirePoint.position;
                obj.transform.forward = Quaternion.Euler(0, i * 45, 0) *Vector3.forward;
                obj.GetComponent<ProjectileCollision>().m_Creator = gameObject;
                Destroy(obj, 10);
            }
        }
    }
}