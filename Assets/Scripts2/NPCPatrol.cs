using UnityEngine;
using System.Collections;

public class NPCPatrol : MonoBehaviour
{
    public Transform leftPoint;  // ���� �� ��ġ
    public Transform rightPoint; // ������ �� ��ġ
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float waitTime = 1f;  // ���� ��ȯ �� ��� �ð�

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 targetPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = rightPoint.position;
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            // ��ǥ ��ġ���� �̵�
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // ���� ��ȯ
            yield return new WaitForSeconds(waitTime);
            if (targetPosition == rightPoint.position)
            {
                targetPosition = leftPoint.position;
                spriteRenderer.flipX = true;  // ���� �ٶ󺸵��� ����
            }
            else
            {
                targetPosition = rightPoint.position;
                spriteRenderer.flipX = false; // ������ �ٶ󺸵���
            }
        }
    }
}
