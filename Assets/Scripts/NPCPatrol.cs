using UnityEngine;
using System.Collections;

public class NPCPatrol : MonoBehaviour
{
    public Transform leftPoint;  // 왼쪽 끝 위치
    public Transform rightPoint; // 오른쪽 끝 위치
    public float moveSpeed = 2f; // 이동 속도
    public float waitTime = 1f;  // 방향 전환 후 대기 시간

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
            // 목표 위치까지 이동
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 방향 전환
            yield return new WaitForSeconds(waitTime);
            if (targetPosition == rightPoint.position)
            {
                targetPosition = leftPoint.position;
                spriteRenderer.flipX = true;  // 왼쪽 바라보도록 반전
            }
            else
            {
                targetPosition = rightPoint.position;
                spriteRenderer.flipX = false; // 오른쪽 바라보도록
            }
        }
    }
}
