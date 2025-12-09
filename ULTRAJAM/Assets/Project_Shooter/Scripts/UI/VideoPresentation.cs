using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPresentation : MonoBehaviour
{
    [SerializeField] private List<VideoClip> videos;
    [SerializeField] private VideoPlayer source;
    [SerializeField] Button next;
    [SerializeField] private List<string> texts;
    [SerializeField] private TMP_Text hint;
    private int currentIndex = 0;
    private void Awake()
    {
        ChangeVideo();
        next.onClick.AddListener(ChangeVideo);
    }

    private void ChangeVideo()
    {
        source.Pause();
        Debug.Log(currentIndex + "  " + videos.Count);
        if (currentIndex >= videos.Count)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }
        hint.text = texts[currentIndex];
        source.clip = videos[currentIndex];
        source.Play();
        currentIndex++;
    }
}
