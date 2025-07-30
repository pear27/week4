using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int damageAmount = 1;        // 사물과 닿을 때 줄 피해량
    public AudioClip hitSound;          // 플레이어와 닿을 때 재생할 소리
    private AudioSource audioSource;    // 소리 재생용 AudioSource

    private void Awake()
    {
        // 오브젝트에 AudioSource가 없으면 자동 추가
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

                // 소리 재생
                if (hitSound != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }
            }
        }
    }
}
