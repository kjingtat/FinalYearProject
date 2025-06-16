using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public Health playerHealth;

    void Update()
    {
        // Check if player or their GameObject is destroyed
        if (playerHealth == null || playerHealth.gameObject == null)
        {
            hpText.text = "HP: 0 (Dead)";
            return;
        }

        hpText.text = "HP: " + playerHealth.CurrentHP;
    }
}
