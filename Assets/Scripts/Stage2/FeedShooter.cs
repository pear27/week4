using UnityEngine;

public class FeedShooter : MonoBehaviour
{
    public GameObject feedPrefab;        // 사료 프리팹
    public Transform spawnPoint;         // 깔대기 입구
    public float shootForce = 7f;        // 발사 힘
    public float shootAngleRange = 45f;  // 각도 범위 (예: ±45도)

    public float shootInterval = 0.5f;   // 발사 간격
    private float timer = 0f;

    public bool isActive = true; // 활성화 여부

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

        // 무작위 방향 설정 (위쪽 + 좌/우로 각도)
        float angle = Random.Range(-shootAngleRange, shootAngleRange);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

        rb.AddForce(direction * shootForce, ForceMode2D.Impulse);
    }
}
