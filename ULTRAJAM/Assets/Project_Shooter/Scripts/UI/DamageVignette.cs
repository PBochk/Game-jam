using UnityEngine;
using UnityEngine.UI;

public class DamageVignette : MonoBehaviour
{
    [SerializeField] private Image vignetteImage;
    [SerializeField] private float maxAlpha = 0.7f;
    [SerializeField] private float fadeSpeed = 3f;

    private float targetAlpha = 0f;
    private float currentAlpha = 0f;

    void Update()
    {
        currentAlpha = Mathf.Lerp(
            currentAlpha,
            targetAlpha,
            Time.deltaTime * fadeSpeed
        );

        Color color = vignetteImage.color;
        color.a = currentAlpha;
        vignetteImage.color = color;
    }

    public void ShowDamageEffect(float intensityMultiplier = 1f, float duration = 0.5f)
    {
        targetAlpha = maxAlpha * intensityMultiplier;
        Invoke(nameof(HideEffect), duration);
    }

    private void HideEffect()
    {
        targetAlpha = 0f;
    }

    // Для интеграции с системой здоровья
    public void OnDamageTaken(int damage, int currentHealth)
    {
        float intensity = Mathf.Clamp01(damage / 100f);
        ShowDamageEffect(intensity);
    }
}