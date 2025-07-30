using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public int damageAmount = 2; // ������Ʈ�� ������ �� ���� ���ط�
    public AudioClip hitGroundClip; // ���� ���� �� ���� �Ҹ�
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // AudioSource�� ������ �ڵ� �߰�
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1) �÷��̾� �浹 ó��
        if (collision.gameObject.CompareTag("Player"))
        {
            GooseWalkingController goose = collision.gameObject.GetComponent<GooseWalkingController>();
            if (goose != null)
            {
                if (goose.IsOnPlatform())
                {
                    Debug.Log("Player is on a platform, no damage taken.");
                    return;
                }
                else
                {
                    PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damageAmount);
                        Debug.Log("Player took damage: " + damageAmount);
                    }
                }
            }
        }

        // 2) ���� �浹�ߴ��� Ȯ��
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (hitGroundClip != null)
            {
                audioSource.PlayOneShot(hitGroundClip);
                Debug.Log("Ground hit sound played.");
            }
        }
    }
}
