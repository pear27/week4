using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int damageAmount = 1;        // �繰�� ���� �� �� ���ط�
    public AudioClip hitSound;          // �÷��̾�� ���� �� ����� �Ҹ�
    private AudioSource audioSource;    // �Ҹ� ����� AudioSource

    private void Awake()
    {
        // ������Ʈ�� AudioSource�� ������ �ڵ� �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player touched object and took damage: " + damageAmount);

                // �Ҹ� ���
                if (hitSound != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }
            }
        }
    }
}
