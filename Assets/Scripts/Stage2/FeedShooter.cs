using UnityEngine;

public class FeedShooter : MonoBehaviour
{
    public GameObject feedPrefab;        // ��� ������
    public Transform spawnPoint;         // ���� �Ա�
    public float shootForce = 7f;        // �߻� ��
    public float shootAngleRange = 45f;  // ���� ���� (��: ��45��)

    public float shootInterval = 0.5f;   // �߻� ����
    private float timer = 0f;

    public bool isActive = true; // Ȱ��ȭ ����

    private void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime;
        if (timer >= shootInterval)
        {
            ShootFeed();
            timer = 0f;
        }
    }

    private void ShootFeed()
    {
        GameObject feed = Instantiate(feedPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = feed.GetComponent<Rigidbody2D>();

        // ������ ���� ���� (���� + ��/��� ����)
        float angle = Random.Range(-shootAngleRange, shootAngleRange);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

        rb.AddForce(direction * shootForce, ForceMode2D.Impulse);
    }
}
