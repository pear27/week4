using UnityEngine;

public class GooseWalkingController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 18f;
    public Transform groundCheck; // �� üũ�� Ʈ������
    public float groundCheckRadius = 0.2f; // �� üũ �ݰ�
    public LayerMask groundLayer; // �� ���̾�
    public LayerMask platformLayer; // ���� �� �ִ� ������Ʈ ���̾�

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private bool isHiding;
    private bool isOnPlatform = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
    }

    private void Update()
    {
        // ���� �ִ��� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // �÷��� ���� �ִ��� üũ
        isOnPlatform = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformLayer);

        // ����� Ű
        isHiding = Input.GetKey(KeyCode.DownArrow);
        animator.SetBool("isHiding", isHiding); // �ִϸ��̼� ���� ������Ʈ


        if (isHiding)
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // �̵� �� ����� ����
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal"); // -1 (����), 0, 1 (������)
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y); // �̵� ó��

        // �ٶ󺸴� ���� ó��
        if (moveX != 0)
        {
            spriteRenderer.flipX = moveX < 0; // �������� �� �� ����
        }

        // �ִϸ��̼� ó��
        bool isWalking = Mathf.Abs(moveX) > 0.01f;
        animator.SetBool("isWalking", isWalking);

        if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public bool IsOnPlatform()
    {
        return isOnPlatform;
    }
}
