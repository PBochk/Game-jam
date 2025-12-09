using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shooter.Gameplay;
using TMPro;

namespace Shooter.UI
{
    public class UI_HUD : MonoBehaviour
    {
        public Image m_PlayerHealth;
        public Image vignette;
        public Animator heartAnimator;
        public TMP_Text current;
        public TMP_Text needed;
        public static UI_HUD m_Main;
        public SpawnControl spawnControl;

        void Awake()
        {
            m_Main = this;
            spawnControl = FindFirstObjectByType<SpawnControl>();
        }

        void Update()
        {
            if (PlayerChar.m_Current == null) return; 
           
            DamageControl damage = PlayerChar.m_Current.GetComponent<DamageControl>();
            m_PlayerHealth.fillAmount = damage.Damage / damage.MaxDamage;

            Color color = vignette.color;
            color.a = 1 - damage.Damage / damage.MaxDamage;
            vignette.color = color;
        
            PlayerPowers p = PlayerChar.m_Current.GetComponent<PlayerPowers>();
            current.text = spawnControl.CountOfKills.ToString();
        }

        public void StartPulse()
        {
            heartAnimator.SetTrigger("StartPulse");
        }
    }
}
