using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;     // 최대 하트 개수
    private int currentHearts;    // 현재 하트 개수

    public UIHeartManager uiHeartManager; // 하트 UI를 업데이트하는 매니저 연결
    public SpriteRenderer spriteRenderer; // 플레이어의 스프라이트 렌더러

    private Color originalColor; // 원래 스프라이트 색상

    private void Start()
    {
        currentHearts = maxHearts;
        uiHeartManager.UpdateHearts(currentHearts); // UI 초기화

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color; // 원래 색상 저장   
    }

    public void TakeDamage(int amount)
    {
        if (currentHearts <= 0) return; // 이미 죽은 상태면 무시

        int oldHearts = currentHearts; // 현재 하트 개수 저장
        currentHearts -= amount;
        if (currentHearts < 0)
            currentHearts = 0;

        if (currentHearts < oldHearts)
        {
            uiHeartManager.BlinkHearts(currentHearts); // 하트 깜빡임 효과
            Debug.Log($"Player took damage! Current hearts: {currentHearts}");
            StartCoroutine(DamageFlash());
        }

        uiHeartManager.UpdateHearts(currentHearts); // UI 갱신

        if (currentHearts == 0)
            GameOver();        
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = Color.red; // 빨간색으로 변경
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
        spriteRenderer.color = originalColor; // 원래 색상으로 복원
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        FadeManager.Instance.FadeOutAndLoad("Dead1"); // Game Over 씬으로 전환
    }
}
