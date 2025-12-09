using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class SpiderSpawner_A : MonoBehaviour
    {
        public Transform m_Base;
        public Transform m_SpawnPoint;
        public GameObject m_EnemyPrefab1;
        // Start is called before the first frame update
        void Start()
        {
            m_Base.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(Co_StartSpawn());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_StartSpawn()
        {
            yield return new WaitForSeconds(2);
            float lerp = 0;
            while(lerp<=1)
            {
                m_Base.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 3.8f, 0), lerp);
                m_Base.localPosition += new Vector3(0.03f*Mathf.Sin(50*Time.time), 0, 0);
                lerp += 0.6f*Time.deltaTime;
                yield return null;
            }

            m_Base.localPosition = new Vector3(0, 3.8f, 0);
            yield return new WaitForSeconds(.5f);

            for (int i = 0; i < 3; i++)
            {
                GameObject obj = Instantiate(m_EnemyPrefab1);
                obj.transform.position = m_SpawnPoint.position;
                obj.transform.forward = Vector3.back ;

                Enemy e = obj.GetComponent<Enemy>();
                e.m_StartWalkDistance = 10;

                yield return new WaitForSeconds(1);
            }
        }
    }

}