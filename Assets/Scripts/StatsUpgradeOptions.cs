using UnityEngine;

[System.Serializable]
public class StatsUpgradeOptions
{
    public string name;
    public string description;

    public enum StatType
    {
        MaxHealth,
        MoveSpeed,
        AttackDamage,
        FireRate,
        InvulnerabilityTime,
        Lifesteal
    }

    public StatType stat;
    public float amount;
}
