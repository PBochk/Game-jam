using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BikeSceneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text bText;

    private void Awake()
    {
        text.alpha = 0f;
        bText.alpha = 0f;
        var temp = button.image.color;
        temp.a = 0f;
        button.image.color = temp;
        //button.onClick.AddListener(() => SceneManager.LoadScene("Ending"));
        button.onClick.AddListener(() =>
        {
            var go = new GameObject("Sacrificial Lamb");
            DontDestroyOnLoad(go);

            foreach(var root in go.scene.GetRootGameObjects())
                Destroy(root);                
            SceneManager.LoadScene("MainMenu");
        });
    }

    private void Update()
    {
        if(text.alpha != 1f)
        {
            text.alpha += 0.002f;
            bText.alpha += 0.002f;
            var temp = button.image.color;
            temp.a += 0.002f;
            button.image.color = temp;
        }
    }
}
