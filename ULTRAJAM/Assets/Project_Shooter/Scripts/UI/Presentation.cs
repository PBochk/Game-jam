using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private List<Sprite> slides;
    [SerializeField] Button next;
    [SerializeField] private Image current;
    [SerializeField] private AudioSource source;
    [SerializeField] private List<string> texts;
    [SerializeField] private TMP_Text desc;
    private int currentIndex = 0;
    private void Awake()
    {
        ChangeSlide();
        next.onClick.AddListener(ChangeSlide);
    }

    private void ChangeSlide()
    {
        source.Play();
        if (currentIndex >= slides.Count)
        {
            SceneManager.LoadScene("ArenaTest");
            return;
        }
        current.sprite = slides[currentIndex];
        desc.text = texts[currentIndex];
        currentIndex++;
    }
}
