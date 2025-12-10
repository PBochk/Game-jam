using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Enemy_SpinMine : Enemy
    {
        public int m_Direction = 1;
        public float m_StartArc = 0;
        // Start is called before the first frame update
        void Start()
        {
            InitPosition = transform.position;
            StartCoroutine(Co_AttackLoop());
        }

        // Update is called once per frame
        void Update()
        {
            HandleDeath();
        }

        IEnumerator Co_AttackLoop()
        {
            while(true)
            {
                float radiusArc = 0;
                //float radius = 0;
                float arc = m_StartArc;

                while(true)
                {
                    arc += 400*Time.deltaTime;
                    radiusArc += Time.deltaTime;
                    Vector3 pos = Helper.RotatedLenght(m_Direction* arc, 3 * Mathf.Sin(radiusArc));
                    transform.position = InitPosition + pos;
                    transform.rotation = Quaternion.Euler(0, -5*m_Direction*arc, 0);
                    if (radiusArc>=Mathf.PI)
                    {
                        break;
                    }
                    yield return null;
                }
                transform.position = InitPosition;

                yield return new WaitForSeconds(1);

            }
        }
    }
}
