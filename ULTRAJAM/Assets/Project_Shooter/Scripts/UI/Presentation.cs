using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private List<Sprite> slides;
    [SerializeField] Button next;
    [SerializeField] private Image current;
    [SerializeField] private AudioSource source;
    private int currentIndex = 0;
    private void Awake()
    {
        ChangeSlide();
        next.onClick.AddListener(ChangeSlide);
    }

    private void ChangeSlide()
    {
        Debug.Log(currentIndex + "  " +  slides.Count);
        source.Play();
        if (currentIndex >= slides.Count)
        {
            SceneManager.LoadScene("ArenaTest");
            return;
        }
        current.sprite = slides[currentIndex];
        currentIndex++;
    }
}
