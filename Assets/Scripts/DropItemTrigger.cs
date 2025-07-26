using UnityEngine;

public class DropItemTrigger : MonoBehaviour
{
    public Rigidbody2D itemToDrop; // ������ ����
    private bool hasDropped = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasDropped)
        {
            hasDropped = true;
            // Rigidbody2D�� Dynamic���� �ٲ㼭 �߷� ����
            itemToDrop.bodyType = RigidbodyType2D.Dynamic;

            itemToDrop.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
            itemToDrop.angularVelocity = Random.Range(-100f, 100f); // ���� ȸ�� �ӵ�
        }
    }
}
