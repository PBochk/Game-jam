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
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private VideoPlayer videoPlayer;
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
        videoPlayer.Pause();
        audioSource.Play();
        if (currentIndex >= videos.Count)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }
        hint.text = texts[currentIndex];
        videoPlayer.clip = videos[currentIndex];
        videoPlayer.Play();
        currentIndex++;
    }
}
