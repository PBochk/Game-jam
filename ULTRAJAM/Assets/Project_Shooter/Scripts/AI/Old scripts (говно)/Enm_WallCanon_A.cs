using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Enm_WallCanon_A : Enemy
    {
        [Space]
        public GameObject m_BulletPrefab1;
        public GameObject m_FireParticlePrefab1;

        public float m_InitDelay = 0;
        public float m_FireDelay = 1;
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

            if (m_InitDelay>0)
                yield return new WaitForSeconds(m_InitDelay);
            while (true)
            {
                
                ShootBullet();
                yield return new WaitForSeconds(m_FireDelay);
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
    }
}
