using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public int damageAmount = 2; // 오브젝트가 떨어질 때 입힐 피해량

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 플레이어인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            GooseWalkingController goose = collision.gameObject.GetComponent<GooseWalkingController>();
            if(goose != null)
            {
                if (goose.IsOnPlatform())
                {
                    Debug.Log("Player is on a platform, no damage taken.");
                    // 플레이어가 밟고 지나가면 피해를 입지 않음
                    return;
                }
                else
                {
                    // 플레이어에게 피해를 입히는 로직
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
