using UnityEngine;

public enum UpgradeType
{
    Health,
    Damage,
    FireRate,
    Invulnerability
}

public class MushUpgrade : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public UpgradeType upgradeType; 
    public float amount = 1f;  
}
