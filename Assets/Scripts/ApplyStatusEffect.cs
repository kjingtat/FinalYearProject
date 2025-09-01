using UnityEngine;

public class ApplyStatusEffect : MonoBehaviour
{
    public EffectType effectType = EffectType.Poison;
    public float duration = 5f;
    public bool applyOnCollision = true;
    public bool applyOnTrigger = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!applyOnCollision) return;
        if (collision.collider.CompareTag("Player"))
        {
            StatusEffectManager status = collision.collider.GetComponent<StatusEffectManager>();
            if (status != null)
            {
                status.ApplyEffect(gameObject, effectType, duration);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!applyOnTrigger) return;
        if (collision.CompareTag("Player"))
        {
            StatusEffectManager status = collision.GetComponent<StatusEffectManager>();
            if (status != null)
            {
                status.ApplyEffect(gameObject, effectType, duration);
            }
            Destroy(gameObject);
        }
    }
}
