using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithDelay : MonoBehaviour
{
    //private Button button;
    private Image image;
    private TMP_Text text;
    public float delay = 3f;
    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponent<TMP_Text>();
        StartCoroutine(WaitForButton());
        image.enabled = false;
    }

    private IEnumerator WaitForButton()
    {
        yield return new WaitForSeconds(delay);
        image.enabled = true;
    }
}