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
            Debug.LogWarning("Player ������Ʈ�� ã�� �� �����ϴ�. �±� Ȯ�� �ʿ�.");
    }

    void Update()
    {
        if (player == null) return;

        // �̵� ���� ���
        Vector3 direction = (player.position - transform.position).normalized;

        // Sprite ���� ����
        if (spriteRenderer != null)
            spriteRenderer.flipX = direction.x < 0;

        // NPC �̵�
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

        // �ȱ� �ִϸ��̼� ����
        if (animator != null)
            animator.SetBool("isWalking", true); // �ʿ� �� ���� ���µ� üũ ����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾�� ���� �� ���� ����
        if (collision.gameObject.CompareTag("Player"))
            FadeManager.Instance.FadeOutAndLoad("Dead2");
    }
}
