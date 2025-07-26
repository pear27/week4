using UnityEngine;

public class DropItemTrigger : MonoBehaviour
{
    public Rigidbody2D itemToDrop; // 떨어질 물건
    private bool hasDropped = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasDropped)
        {
            hasDropped = true;
            // Rigidbody2D를 Dynamic으로 바꿔서 중력 적용
            itemToDrop.bodyType = RigidbodyType2D.Dynamic;

            itemToDrop.velocity = Vector2.zero; // 속도 초기화
            itemToDrop.angularVelocity = Random.Range(-100f, 100f); // 랜덤 회전 속도
        }
    }
}
