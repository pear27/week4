using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int damageAmount = 1; // �繰�� ���� �� �� ���ط�

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
