using UnityEngine;

public class StartExplosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    //[SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        particles.Play();
    }
}
