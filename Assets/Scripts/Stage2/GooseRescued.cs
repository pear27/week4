using UnityEngine;

public class GooseRescued : MonoBehaviour
{
    public float walkSpeed = 1f;
    private Animator animator;
    private bool isWalking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Invoke(nameof(StartWalking), 1f); // 1�� �� �ȱ� ����
    }

    private void Update()
    {
        if (isWalking)
        {
            transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);
        }
    }

    private void StartWalking()
    {
        isWalking = true;
        if (animator != null)
        {
            animator.SetTrigger("Walk"); // "Walk" Ʈ���Ű� �ִ� �ִϸ��̼� Ŭ�� �ʿ�
        }
    }
}
