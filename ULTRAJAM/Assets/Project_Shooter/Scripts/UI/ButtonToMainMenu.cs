using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonToMainMenu : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }
}
