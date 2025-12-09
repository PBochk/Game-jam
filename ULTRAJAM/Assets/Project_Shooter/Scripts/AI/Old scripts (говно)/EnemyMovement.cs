using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class EnemyMovement : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 m_MovementDirection;
        [HideInInspector]
        public Vector3 m_MovementTargetPosition;
        [HideInInspector]
        public bool m_MovementEnable = true;
        public float m_MovementSpeed = 20;
        [HideInInspector]
        public bool m_ReachedTargetPosition = false;
        [HideInInspector]
        public bool m_FaceMoveDirection = true;

        [HideInInspector]
        public bool m_UsePhysics = true;

        public Animator m_Animator;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!m_ReachedTargetPosition)
            {
                if (m_MovementTargetPosition != Vector3.zero)
                {
                    m_MovementDirection = m_MovementTargetPosition - transform.position;
                    m_MovementDirection.y = 0;
                    
                    if (m_MovementDirection.magnitude <= .5f)
                    {
                        m_MovementTargetPosition = Vector3.zero;
                        m_MovementDirection = Vector3.zero;
                        m_ReachedTargetPosition = true;
                        
                    }
                    m_MovementDirection.Normalize();
                    if (m_FaceMoveDirection && m_MovementDirection!=Vector3.zero)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_MovementDirection), 10 * Time.deltaTime);
                    }
                }
            }

            //if (!m_UsePhysics)
            //{
                if (m_MovementDirection != Vector3.zero)
                {
                    transform.position += Time.deltaTime * m_MovementSpeed * m_MovementDirection;
                    if (m_Animator!=null)
                        m_Animator.SetFloat("WalkSpeed", 1);
                }
                else
                {
                    if (m_Animator != null)
                        m_Animator.SetFloat("WalkSpeed", 0);
                }
            //}
        }

        public void SetMoveTargetPosition(Vector3 position)
        {
            m_ReachedTargetPosition = false;
            m_MovementTargetPosition = position;
        }

        void FixedUpdate()
        {
            //if (m_UsePhysics)
            //{
            //    Rigidbody rigidBody = GetComponent<Rigidbody>();

            //    Vector3 totalVelocity = rigidBody.velocity;
            //    if (m_MovementDirection != Vector3.zero)
            //    {
            //        totalVelocity = m_MovementSpeed * m_MovementDirection;
            //        totalVelocity.y = -10;
            //        //totalVelocity = Vector3.ClampMagnitude(totalVelocity, 10);
            //        totalVelocity.y = rigidBody.velocity.y;
            //        rigidBody.velocity = totalVelocity;
            //    }
            //    else
            //    {
            //        totalVelocity -= 0.1f * totalVelocity;
            //        totalVelocity.y = rigidBody.velocity.y;
            //        rigidBody.velocity = totalVelocity;
            //    }
            //}
        }
    }
}