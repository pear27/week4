using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public int damageAmount = 2; // ������Ʈ�� ������ �� ���� ���ط�

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������Ʈ�� �÷��̾����� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            GooseWalkingController goose = collision.gameObject.GetComponent<GooseWalkingController>();
            if(goose != null)
            {
                if (goose.IsOnPlatform())
                {
                    Debug.Log("Player is on a platform, no damage taken.");
                    // �÷��̾ ��� �������� ���ظ� ���� ����
                    return;
                }
                else
                {
                    // �÷��̾�� ���ظ� ������ ����
                    // PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                    //if (playerHealth != null)
                    //{
                        // playerHealth.TakeDamage(damageAmount);
                        Debug.Log("Player is not on a platform, damage taken.");
                    //}
                }
            }
        }
    }
}
