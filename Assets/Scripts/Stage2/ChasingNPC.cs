using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChasingNPC : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public int maxHealth = 40;
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor; // ���� ��������Ʈ ����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color; // ���� ���� ����   
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
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = Color.red; // ���������� ����
        yield return new WaitForSeconds(0.1f); // 0.1�� ���
        spriteRenderer.color = originalColor; // ���� �������� ����
    }

    void Die()
    {
        // �ִϸ��̼� �߰� �����̸� ���⼭ Ʈ���� ����
        Debug.Log("NPC died");
        Destroy(gameObject);
    }
}
