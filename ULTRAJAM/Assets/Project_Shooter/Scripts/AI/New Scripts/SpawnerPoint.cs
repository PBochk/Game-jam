using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class SpawnerPoint : MonoBehaviour
    {
        public Transform baseTransform;

        public void Awake()
        {
            baseTransform = transform;
        }

        public GameObject SpawnEnemy(GameObject enemy)
        {
            return Instantiate(enemy, baseTransform);
        }

    }

}