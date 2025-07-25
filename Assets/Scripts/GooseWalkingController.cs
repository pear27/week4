using UnityEngine;

public class GooseWalkingController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
    }
}
