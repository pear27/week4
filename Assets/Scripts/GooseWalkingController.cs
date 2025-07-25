using UnityEngine;

public class GooseWalkingController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 8f;
    public Transform groundCheck; // 땅 체크용 트랜스폼
    public float groundCheckRadius = 0.1f; // 땅 체크 반경
    public LayerMask groundLayer; // 땅 레이어

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
        float moveX = Input.GetAxisRaw("Horizontal"); // -1 (왼쪽), 0, 1 (오른쪽)

        // 이동 처리
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // 애니메이션 처리
        bool isWalking = Mathf.Abs(moveX) > 0.01f;
        animator.SetBool("isWalking", isWalking);

        // 바라보는 방향 처리
        if (moveX != 0)
        {
            spriteRenderer.flipX = moveX < 0; // 왼쪽으로 갈 때 반전
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
