using UnityEngine;

public class GooseWalkingController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 8f;
    public Transform groundCheck; // �� üũ�� Ʈ������
    public float groundCheckRadius = 0.1f; // �� üũ �ݰ�
    public LayerMask groundLayer; // �� ���̾�

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // -1 (����), 0, 1 (������)

        // �̵� ó��
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // �ִϸ��̼� ó��
        bool isWalking = Mathf.Abs(moveX) > 0.01f;
        animator.SetBool("isWalking", isWalking);

        // �ٶ󺸴� ���� ó��
        if (moveX != 0)
        {
            spriteRenderer.flipX = moveX < 0; // �������� �� �� ����
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
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
}
