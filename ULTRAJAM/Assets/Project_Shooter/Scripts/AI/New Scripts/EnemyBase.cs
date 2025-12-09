using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        [SerializeField] private Transform firePoint; 
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private GameObject enemyProjectilePrefab;
        [SerializeField] private float projectileSpeed = 40f;
        [SerializeField] private float collisionDamage = 10f;
        [SerializeField] private float collisionKnockbackForce = 5f;


        private static float playerInvulnerabilityTimer = 0f;
        private const float INVULNERABILITY_DURATION = 1f;

        private float currentAttackTimer = 0f;
        private bool canAttack = true;




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

            if (playerInvulnerabilityTimer > 0)
            {
                playerInvulnerabilityTimer -= Time.fixedDeltaTime;
            }


            if (DamageControl.IsDead && !IsDead)
            {
                HandleDeath();
                return;
            }

            if (!canAttack)
            {
                currentAttackTimer -= Time.fixedDeltaTime;
                if (currentAttackTimer <= 0)
                {
                    canAttack = true;
                }
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
                RotateTowardsPlayer();

            }
            if (canAttack)
                Attacking();
        }

        public void OnCollisionStay(Collision collision)
        {
            var obj = collision.gameObject;
            if (!obj.CompareTag("Player")) return;

            if (playerInvulnerabilityTimer > 0) return;

            ApplyCollisionDamage(obj, collision);
        }


        private void ApplyCollisionDamage(GameObject player, Collision collision)
        {
            DamageControl playerDC = player.GetComponent<DamageControl>();
            if (playerDC != null && !playerDC.IsDead)
            {
                var direction = (player.transform.position - transform.position).normalized;

                playerDC.ApplyDamage(collisionDamage, direction, 1f);

                playerInvulnerabilityTimer = INVULNERABILITY_DURATION;

                ApplyKnockback(player, direction);
            }
        }

        private void ApplyKnockback(GameObject player, Vector3 direction)
        {
            var playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                direction.y = 0.3f;
                playerRb.AddForce(direction * collisionKnockbackForce, ForceMode.Impulse);
            }
        }

        private void Attacking()
        {
            if (enemyProjectilePrefab is null)
            {
                Debug.LogWarning("EnemyProjectilePrefab не назначен!");
                return;
            }

            canAttack = false;  
            currentAttackTimer = attackCooldown;

            if (Animator != null)
            {
                Animator.SetTrigger("Attack");
            }

            var directionToPlayer = (PlayerTarget.position - transform.position).normalized;

            SpawnProjectile(directionToPlayer);
        }

        private void SpawnProjectile(Vector3 direction)
        {
            Vector3 spawnPosition;
            if (firePoint != null)
                spawnPosition = firePoint.position;
            else
                spawnPosition = transform.position + Vector3.up * 1.5f;

            var projectile = Instantiate(
                enemyProjectilePrefab,
                spawnPosition,
                Quaternion.LookRotation(direction)
            );

            var proj = projectile.GetComponent<Projectile_Base>();
            if (proj != null)
            {
                proj.Creator = gameObject;
                proj.Speed = projectileSpeed;
                proj.Damage = Damage; 
                proj.m_Range = distanceToAttack * 1.5f;
            }

            var projCollision = projectile.GetComponent<ProjectileCollision>();
            if (projCollision != null)
            {
                projCollision.m_Creator = gameObject;
                projCollision.m_Damage = Damage;
                projCollision.m_IsEnemyTeam = true; 
            }
        }

        public virtual void HandleDeath()
        {
            if (IsDead) return;

            IsDead = true;
            var distanceToPlayer = Vector3.Distance(transform.position, PlayerTarget.position);

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