using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;

    private Health health;

    void Start()
    {
        health = GetComponentInParent<Health>();

        if (health != null)
        {
            health.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(health.CurrentHP, health.MaxHP); 
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Could not find Health component!", this);
        }
    }


    private void UpdateHealthBar(int current, int max)
    {
        if (healthFillImage != null)
            healthFillImage.fillAmount = (float)current / max;
    }

    void OnDestroy()
    {
        if (health != null)
        {
            health.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
