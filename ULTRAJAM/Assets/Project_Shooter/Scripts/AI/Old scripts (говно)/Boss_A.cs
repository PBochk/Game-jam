using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Boss_A : Enemy
    {
        
        bool CanFire = true;

        public Transform ShotPoint;
        public GameObject m_BulletPrefab1;

        public Transform[] m_FirePoints;
        public Transform[] m_DeathParticlePoints;
        [SerializeField]
        protected GameObject m_DeathParticlePrefab2;
        [HideInInspector]
        public int m_AttackLevel = 0;
        // Start is called before the first frame update
        void Start()
        {
            m_DamageControl = GetComponent<DamageControl>();
            InitPosition = transform.position;
            m_DamageControl.m_NoDamage = true;
            transform.position = InitPosition + new Vector3(0, 0, 20);

            m_AttackLevel = 0;
        }

        // Update is called once per frame
        void Update()
        {
            float damage = m_DamageControl.Damage / m_DamageControl.MaxDamage;
            if (damage>.6f)
            {
                m_AttackLevel = 0;
            }
            else if (damage > .3f)
            {
                m_AttackLevel = 1;
            }
            else
            {
                m_AttackLevel = 2;
            }

            HandleFacePlayer();
            HandleDeath();
        }


        IEnumerator Co_AttackLoop()
        {
            while (true)
            {
                switch (m_AttackLevel)
                {
                    case 0:
                        yield return new WaitForSeconds(1f);
                        ShootRingBullet(2);
                        yield return new WaitForSeconds(2f);
                        for (int i = 0; i < 5; i++)
                        {
                            ShootBullet();
                            yield return new WaitForSeconds(.3f);
                        }
                        yield return new WaitForSeconds(2f);
                        break;

                    case 1:
                        yield return new WaitForSeconds(1f);
                        ShootRingBullet(3);
                        yield return new WaitForSeconds(.7f);
                        ShootRingBullet(3);
                        yield return new WaitForSeconds(2f);
                        for (int i = 0; i < 10; i++)
                        {
                            ShootBullet();
                            yield return new WaitForSeconds(.2f);
                        }
                        yield return new WaitForSeconds(1f);
                        break;

                    case 2:
                        yield return new WaitForSeconds(1f);
                        ShootRingBullet(4);
                        yield return new WaitForSeconds(.6f);
                        ShootRingBullet(4);
                        yield return new WaitForSeconds(.6f);
                        ShootRingBullet(4);
                        yield return new WaitForSeconds(2f);
                        for (int i = 0; i < 15; i++)
                        {
                            ShootBullet();
                            yield return new WaitForSeconds(.3f);
                        }
                        yield return new WaitForSeconds(1f);
                        break;
                }
            }
            //yield return null;
        }
        IEnumerator Co_MoveLoop()
        {
            EnemyMovement movement = GetComponent<EnemyMovement>();
            StateSystem stateSys = GetComponent<StateSystem>();

            movement.m_FaceMoveDirection = false;
            m_FacePlayer = true;

            Vector3[] points = new Vector3[4];
            points[0] = InitPosition + new Vector3(-3, 0, 0);
            points[1] = InitPosition + new Vector3(3, 0, 0);
            points[2] = InitPosition + new Vector3(-3, 0, 2);
            points[3] = InitPosition + new Vector3(3, 0, 2);
            int pointNum = 0;
            while (true)
            {
                //movement.m_FaceMoveDirection = true;
                //m_FacePlayer = false;
                movement.SetMoveTargetPosition(points[pointNum]);
                while (!movement.m_ReachedTargetPosition)
                {
                    yield return null;
                }

                pointNum = Random.Range(0, 4);

                yield return new WaitForSeconds(1);

            }
        }


        public override void EnableEnemy()
        {
            base.EnableEnemy();
            StartCoroutine(Co_EnableEnemy());
        }

        IEnumerator Co_EnableEnemy()
        {
            float lerp = 0;
            while(lerp<=1)
            {
                lerp += 0.2f*Time.deltaTime;
                transform.position = Vector3.Lerp(InitPosition + new Vector3(0, 0, 20), InitPosition , lerp);
                yield return null;
            }

            transform.position = InitPosition;

            yield return new WaitForSeconds(1f);
            m_DamageControl.m_NoDamage = false;
            m_FacePlayer = true;
            StartCoroutine(Co_MoveLoop());
            yield return new WaitForSeconds(1f);
            StartCoroutine(Co_AttackLoop());
        }

        public void ShootBullet()
        {
            Vector3 dir;
            GameObject obj;

            obj = Instantiate(m_BulletPrefab1);
            obj.transform.position = m_FirePoints[1].position;
            dir = PlayerChar.m_Current.transform.position - m_FirePoints[1].position;
            dir.y = 0;
            obj.transform.forward = Quaternion.Euler(0,-00,0)* dir;
            obj.GetComponent<ProjectileCollision>().m_Creator = gameObject;
            obj.GetComponent<ProjectileMovement>().m_TurnSpeed = 00;
            Destroy(obj, 10);

            obj = Instantiate(m_BulletPrefab1);
            obj.transform.position = m_FirePoints[2].position;
            dir = PlayerChar.m_Current.transform.position - m_FirePoints[2].position;
            dir.y = 0;
            obj.transform.forward = Quaternion.Euler(0, 00, 0) * dir;
            obj.GetComponent<ProjectileCollision>().m_Creator = gameObject;
            obj.GetComponent<ProjectileMovement>().m_TurnSpeed = -00;
            Destroy(obj, 10);

        }

        public void ShootRingBullet(int halfCount)
        {
            for (int i = -halfCount; i <= halfCount; i++)
            {
                GameObject obj = Instantiate(m_BulletPrefab1);
                obj.transform.position = m_FirePoints[0].position;
                obj.transform.forward = Quaternion.Euler(0, i * 20, 0) * m_RotationBase.forward;
                obj.GetComponent<ProjectileCollision>().m_Creator = gameObject;
                Destroy(obj, 10);
            }
        }

        public override void HandleDeath()
        {
            if (!m_IsDead)
            {
                if (m_DamageControl.Damage <= 0)
                {
                    //base.HandleDeath();
                    StopAllCoroutines();
                    StartCoroutine(Co_HandleDeath());
                    m_IsDead = true;
                }
            }
        }

        IEnumerator Co_HandleDeath()
        {
            float delay = .2f;
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(m_DeathParticlePrefab);
                obj.transform.position = m_DeathParticlePoints[i%5].position;
                Destroy(obj, 10);
                yield return new WaitForSeconds(delay);
                delay -= .01f;
            }

            yield return new WaitForSeconds(.5f);

            GameObject obj1 = Instantiate(m_DeathParticlePrefab2);
            obj1.transform.position = transform.position;
            Destroy(obj1, 6);
            CameraControl.m_Current.StartShake(.6f, .2f);
            DropItem(20);

            Destroy(gameObject);
        }

        public override void DropItem(int count)
        {
            //base.DropItem();
            for (int i = 0; i < 20; i++)
            {
                GameObject obj1 = Instantiate(m_ItemPrefabs[0]);
                obj1.transform.position = transform.position;
                Vector3 v = Helper.RotatedLenght(i * 18, 10) + new Vector3(0, 15, 0);
                obj1.GetComponent<Rigidbody>().linearVelocity = v;
            }

            for (int i = 0; i < 20; i++)
            {
                GameObject obj1 = Instantiate(m_ItemPrefabs[0]);
                obj1.transform.position = transform.position;
                Vector3 v = Helper.RotatedLenght(i * 18, 15) + new Vector3(0, 20, 0);
                obj1.GetComponent<Rigidbody>().linearVelocity = v;
            }
        }
    }
}
