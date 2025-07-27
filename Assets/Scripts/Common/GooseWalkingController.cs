using UnityEngine;

public class GooseWalkingController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 18f;
    public Transform groundCheck; // 땅 체크용 트랜스폼
    public float groundCheckRadius = 0.2f; // 땅 체크 반경
    public LayerMask groundLayer; // 땅 레이어
    public LayerMask platformLayer; // 밟을 수 있는 오브젝트 레이어

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
        // 땅에 있는지 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 플랫폼 위에 있는지 체크
        isOnPlatform = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformLayer);

        // 숨기기 키
        isHiding = Input.GetKey(KeyCode.DownArrow);
        animator.SetBool("isHiding", isHiding); // 애니메이션 상태 업데이트


        if (isHiding)
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // 이동 중 숨기기 방지
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal"); // -1 (왼쪽), 0, 1 (오른쪽)
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y); // 이동 처리

        // 바라보는 방향 처리
        if (moveX != 0)
        {
            spriteRenderer.flipX = moveX < 0; // 왼쪽으로 갈 때 반전
        }

        // 애니메이션 처리
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
