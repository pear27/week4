using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChasingNPC : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public int maxHealth = 10;
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player ������Ʈ�� ã�� �� �����ϴ�. �±� Ȯ�� �ʿ�.");
        }
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
        // 1. �÷��̾�� ���� �� ���� ����
        if (collision.gameObject.CompareTag("Player"))
            FadeManager.Instance.FadeOutAndLoad("Dead2");

        // 2. �������� �ִ� ������Ʈ�� ����
        if (collision.gameObject.CompareTag("Hazard"))
            TakeDamage(1); // �Ǵ� Hazard�� �Ӽ����� ������ �� ����
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"NPC took damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // �ִϸ��̼� �߰� �����̸� ���⼭ Ʈ���� ����
        Debug.Log("NPC died");
        Destroy(gameObject);
    }
}
