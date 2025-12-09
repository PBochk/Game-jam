using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class TargetObject : MonoBehaviour
    {
        public Transform m_TargetCenter;
        // Start is called before the first frame update
        void Start()
        {
            TargetsControl.m_Main.AddTarget(this);
            if (m_TargetCenter==null)
            {
                m_TargetCenter = transform;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDeactivate()
        {
            RemoveFromTargets();
        }
        public void RemoveFromTargets()
        {
            TargetsControl.m_Main.RemoveTarget(this);
        }
    }

}