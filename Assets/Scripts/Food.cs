using UnityEngine;

public class Food : MonoBehaviour
{
    public int healAmount = 5;

    public void Consume(Health playerHealth)
    {
        if (playerHealth != null)
        {
            Debug.Log("Food consumed! Healing " + healAmount);
            playerHealth.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
