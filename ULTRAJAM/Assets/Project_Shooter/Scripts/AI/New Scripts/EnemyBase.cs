using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Gameplay
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private int maxHp = 100;
        private int currentHp = 100;
        [SerializeField] private GameObject Prefab;
        private Transform PlayerTarget;
        private DamageControl DamageControl;
        [SerializeField] private float distanceToAttack;
        [SerializeField] private float speed;
        [SerializeField] private float attackSpeed;
        [SerializeField] private Rigidbody RigidBody;
        [SerializeField] private int Damage = 10;
        [SerializeField] private GameObject DeathParticlePrefab;




        //AI
        bool isMoving = true;
        bool isAttacking = false;
        public bool IsDead = false;
        public bool nAlerted = false;
        public Animator Animator;


        void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
            DamageControl = GetComponent<DamageControl>();
            DamageControl.OnDamaged.AddListener(CheckDeath);
        }
        public void Start()
        {
            PlayerTarget = PlayerControl.MainPlayerController.MyPlayerChar.transform; 
        }

        void FixedUpdate()
        {
            if (DamageControl.IsDead && !IsDead)
            {
                HandleDeath();
                return;
            }

            var playerPosition = PlayerTarget.position;
            var myPosition = transform.position;
            var distanceToPlayer = Vector3.Distance(transform.position, PlayerTarget.position);
            if (distanceToPlayer > distanceToAttack)
            {
                Move();
                RotateTowardsPlayer();
            }

            else
            {
                StopMovement();
                Attacking();
            }
        }

        private void Attacking()
        {

        }
        public virtual void HandleDeath()
        {
            if (IsDead) return;

            IsDead = true;

            Debug.Log($"HandleDeath вызван в {this}");
            var obj = Instantiate(DeathParticlePrefab);
            obj.transform.position = transform.position;
            var pr = obj.GetComponent<ParticleSystem>();
            pr.Play();
            Destroy(obj, 3);
            Destroy(gameObject);
        }


        public void Move()
        {
            isMoving = true;

            var direction = (PlayerTarget.position - transform.position).normalized;
            direction.y = 0;

            var distanceToPlayer = Vector3.Distance(transform.position, PlayerTarget.position);
            var approachSpeed = speed;
            if (distanceToPlayer < distanceToAttack + 2f)
                approachSpeed *= Mathf.Clamp01((distanceToPlayer - distanceToAttack) / 2f);

            RigidBody.linearVelocity = direction * approachSpeed;
            RotateTowardsPlayer();
        }

        private void StopMovement() => RigidBody.linearVelocity = Vector3.zero;

        private void CheckDeath()
        {
            Debug.Log("CheckDeath вызвался");
            Debug.Log($"Damage: {DamageControl.Damage}, IsDead: {DamageControl.IsDead}");

            if (DamageControl.IsDead && !IsDead)
            {
                HandleDeath();
            }
        }
        private void RotateTowardsPlayer()
        {
            var direction = PlayerTarget.position - transform.position;
            direction.y = 0;

            if (direction.magnitude > 0.1f)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    10f * Time.deltaTime
                );
            }
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
        }
    }

}