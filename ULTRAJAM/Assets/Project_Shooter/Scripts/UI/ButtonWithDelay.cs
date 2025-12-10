using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithDelay : MonoBehaviour
{
    //[SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    public float delay = 3f;
    private void Awake()
    {
        //Debug.Log(button == null);
        image.enabled = false;
        text.enabled = false;
        StartCoroutine(WaitForButton());
    }

    private IEnumerator WaitForButton()
    {
        yield return new WaitForSeconds(delay);
        image.enabled = true;
        text.enabled = true;
    }
}