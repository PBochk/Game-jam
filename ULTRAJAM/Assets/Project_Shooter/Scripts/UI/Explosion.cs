using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
