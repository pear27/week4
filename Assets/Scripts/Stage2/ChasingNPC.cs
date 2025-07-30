using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ChasingNPC : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public int maxHealth = 20;
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor; // ���� ��������Ʈ ����

    private TMP_Text hpText;

    public Sprite deadSprite; // ������ ���� ��������Ʈ
    // public GameObject keyPrefab; // ���� ���� ������

    private bool isDead = false;
    private bool canGiveKey = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color; // ���� ���� ����   
        currentHealth = maxHealth;
        
        hpText = GetComponentInChildren<TMP_Text>();
        UpdateHealthUI();

        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Player ������Ʈ�� ã�� �� �����ϴ�. �±� Ȯ�� �ʿ�.");
    }

    void Update()
    {
        if (isDead) return; // NPC�� ���� ��� �̵� �� �ִϸ��̼� ����

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
        if (isDead) return; // NPC�� ���� ��� �浹 ó�� ����

        // 1. �÷��̾�� ���� �� ���� ����
        if (collision.gameObject.CompareTag("Player"))
            FadeManager.Instance.FadeOutAndLoad("Dead2");

        // 2. �������� �ִ� ������Ʈ�� ����
        if (collision.gameObject.CompareTag("Hazard"))
            TakeDamage(1); // �Ǵ� Hazard�� �Ӽ����� ������ �� ����
        
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // �̹� ���� ��� ����

        currentHealth -= amount;
        Debug.Log($"NPC took damage. Remaining HP: {currentHealth}");

        UpdateHealthUI();
        
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
            Die();
    }

    private void UpdateHealthUI()
    {
        if (hpText != null)
            hpText.text = $"{currentHealth}/{maxHealth}";
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
        isDead = true;
        Debug.Log("NPC died");
        
        animator.enabled = false; // �ִϸ��̼� ����
        if (deadSprite != null)
            spriteRenderer.sprite = deadSprite; // ������ ���� ��������Ʈ�� ����

        if (hpText != null)
        {
            hpText.text = $"�����̽��ٸ� ���� ���� ���";
            hpText.gameObject.SetActive(false);
        }

        rb.velocity = Vector2.zero; // �̵� ����
        rb.bodyType = RigidbodyType2D.Static; // ���� �������� NPC ����

        canGiveKey = true; // ���踦 �� �� �ִ� ���·� ����
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDead && canGiveKey && other.CompareTag("Player"))
        {
            if (hpText != null)
                hpText.gameObject.SetActive(true); // �÷��̾ NPC ��ó�� ���� �� HP �ؽ�Ʈ ǥ��
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canGiveKey = false; // ���踦 �� �� �ִ� ���� ����
                Debug.Log("���踦 ������ϴ�!");
            }
        }
    }
}
