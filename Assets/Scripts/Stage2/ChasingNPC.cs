using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ChasingNPC : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다. 태그 확인 필요.");
    }

    void Update()
    {
        if (player == null) return;

        // 이동 방향 계산
        Vector3 direction = (player.position - transform.position).normalized;

        // Sprite 방향 반전
        if (spriteRenderer != null)
            spriteRenderer.flipX = direction.x < 0;

        // NPC 이동
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

        // 걷기 애니메이션 적용
        if (animator != null)
            animator.SetBool("isWalking", true); // 필요 시 정지 상태도 체크 가능
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 접촉 → 게임 오버
        if (collision.gameObject.CompareTag("Player"))
            FadeManager.Instance.FadeOutAndLoad("Dead2");
    }
}
