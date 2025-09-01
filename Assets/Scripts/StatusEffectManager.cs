using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectType { Poison, Slow }

public class StatusEffectManager : MonoBehaviour
{
    private Health health;
    private PlayerStats stats;

    private Dictionary<EffectType, Dictionary<GameObject, float>> activeEffects = new Dictionary<EffectType, Dictionary<GameObject, float>>();
    private Coroutine effectTickRoutine;
    public float tickInterval = 1f;

    void Awake()
    {
        health = GetComponent<Health>();
        stats = GetComponent<PlayerStats>();
        foreach (EffectType type in System.Enum.GetValues(typeof(EffectType)))
        {
            activeEffects[type] = new Dictionary<GameObject, float>();
        }
    }

    public void ApplyEffect(GameObject source, EffectType type, float duration)
    {
        if (!activeEffects[type].ContainsKey(source))
        {
            activeEffects[type][source] = duration;
        }
        else
        {
            activeEffects[type][source] = Mathf.Max(activeEffects[type][source], duration);
        }

        if (effectTickRoutine == null)
            effectTickRoutine = StartCoroutine(EffectTickLoop());
    }

    private IEnumerator EffectTickLoop()
    {
        while (AnyActiveEffect())
        {
            ApplyPoisonEffects();
            ApplySlowEffects();

            foreach (EffectType type in System.Enum.GetValues(typeof(EffectType)))
            {
                List<GameObject> expired = new List<GameObject>();
                var sources = new List<GameObject>(activeEffects[type].Keys);

                foreach (GameObject source in sources)
                {
                    activeEffects[type][source] -= tickInterval;
                    if (activeEffects[type][source] <= 0f)
                        expired.Add(source);
                }

                foreach (GameObject s in expired)
                    activeEffects[type].Remove(s);
            }

            yield return new WaitForSeconds(tickInterval);
        }

        effectTickRoutine = null;
        ResetSpeed();
    }

    private void ApplyPoisonEffects()
    {
        if (activeEffects[EffectType.Poison].Count > 0)
        {
            health.TakeDamage(1, ignoreInvuln: true);
        }
    }

    private void ApplySlowEffects()
    {
        if (stats == null) return;
        float slowMultiplier = 1f;
        foreach (var _ in activeEffects[EffectType.Slow])
            slowMultiplier *= 0.5f;

        stats.moveSpeed = stats.baseMoveSpeed * slowMultiplier;
    }

    private void ResetSpeed()
    {
        if (stats == null) return;
        stats.moveSpeed = stats.baseMoveSpeed;
    }

    private bool AnyActiveEffect()
    {
        foreach (EffectType type in System.Enum.GetValues(typeof(EffectType)))
        {
            if (activeEffects[type].Count > 0)
                return true;
        }
        return false;
    }
}
