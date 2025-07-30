using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int damageAmount = 1; // 사물과 닿을 때 줄 피해량

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player touched object and took damage: " + damageAmount);
            }
        }
    }
}
