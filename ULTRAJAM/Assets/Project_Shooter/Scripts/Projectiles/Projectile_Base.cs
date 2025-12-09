using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Gameplay
{
    public class Projectile_Base : MonoBehaviour
    {
        public GameObject HitParticlePrefab1;
        [HideInInspector]
        public GameObject Creator;

        public float Speed = 100;
        public float Damage = 1;
        public float m_Radius = 1f;
        public float m_Range = 10;

        Vector3 m_StartPosition;
        // Use this for initialization
        void Start()
        {
            m_StartPosition = transform.position;
        }

        void Update()
        {
            if (Vector3.Distance(m_StartPosition,transform.position)>=m_Range)
            {
                Destroy(gameObject);
                return;
            }

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, m_Radius, transform.forward, Speed * Time.deltaTime);
            foreach (RaycastHit hit in hits)
            {
                Collider col = hit.collider;

                if (col.gameObject.tag == "Player")
                {
                    if (col.gameObject != Creator)
                    {
                        DamageControl d = col.gameObject.GetComponent<DamageControl>();
                        if (d != null)
                        {
                            d.ApplyDamage(Damage, transform.forward, 1);
                        }
                        PlayerChar p = col.gameObject.GetComponent<PlayerChar>();
                        Destroyed(hit.point);
                    }
                }
                else if (col.gameObject.tag == "Block")
                {

                    DamageControl d = col.gameObject.GetComponent<DamageControl>();
                    if (d != null)
                    {
                        d.ApplyDamage(Damage, transform.forward, 1);
                    }
                    Destroyed(hit.point);
                }
                else if (col.gameObject.tag == "Enemy")
                {

                    DamageControl d = col.gameObject.GetComponent<DamageControl>();
                    if (d != null)
                    {
                        d.ApplyDamage(Damage, transform.forward, 1);
                    }
                    Destroyed(hit.point);
                }

            }

            transform.position += Speed * Time.deltaTime * transform.forward;
        }

        public virtual void Destroyed(Vector3 pos)
        {
            Destroy(gameObject);
            GameObject obj = Instantiate(HitParticlePrefab1);
            obj.transform.position = pos;
            //obj.transform.localScale = 0.4f * Vector3.one;
            Destroy(obj, 6);
        }
    }
}