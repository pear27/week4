using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public int damageAmount = 2; // 오브젝트가 떨어질 때 입힐 피해량
    public AudioClip hitGroundClip; // 땅에 닿을 때 나는 소리
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // AudioSource가 없으면 자동 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1) 플레이어 충돌 처리
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

        // 2) 땅에 충돌했는지 확인
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
