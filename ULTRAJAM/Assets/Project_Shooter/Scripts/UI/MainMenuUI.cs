using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shooter.ScriptableObjects;
using UnityEngine.UI;
namespace Shooter
{
    public class MainMenuUI : MonoBehaviour
    {
        public SaveData m_SaveData;
        public Button startButton;
        public Button howButton;
        private void Awake()
        {
            startButton.onClick.AddListener(BtnStart);
            howButton.onClick.AddListener(HowBtn);
        }

        void Start()
        {
            m_SaveData.Load();
        }

        public void BtnStart()
        {
            SceneManager.LoadScene("Intro");
        }
        public void HowBtn()
        {
            SceneManager.LoadScene("HowToPlay");
        }
    }
}