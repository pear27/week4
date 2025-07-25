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
    }
}
