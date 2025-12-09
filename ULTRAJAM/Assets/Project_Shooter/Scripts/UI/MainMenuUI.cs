using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shooter.ScriptableObjects;
namespace Shooter
{
    public class MainMenuUI : MonoBehaviour
    {
        public SaveData m_SaveData;
        // Start is called before the first frame update
        void Start()
        {
            m_SaveData.Load();
        }

        public void BtnStart()
        {
            SceneManager.LoadScene("Level_1");
        }
    }
}