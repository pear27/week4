using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;     // 최대 하트 개수
    private int currentHearts;    // 현재 하트 개수

    public UIHeartManager uiHeartManager; // 하트 UI를 업데이트하는 매니저 연결

    private void Start()
    {
        currentHearts = maxHearts;
        uiHeartManager.UpdateHearts(currentHearts); // UI 초기화
    }

    public void TakeDamage(int amount)
    {
        if (currentHearts <= 0) return; // 이미 죽은 상태면 무시

        int oldHearts = currentHearts; // 현재 하트 개수 저장
        currentHearts -= amount;
        if (currentHearts < 0) currentHearts = 0;

        if (currentHearts < oldHearts)
        {
            uiHeartManager.BlinkHearts(currentHearts); // 하트 깜빡임 효과
            Debug.Log($"Player took damage! Current hearts: {currentHearts}");
        }

        uiHeartManager.UpdateHearts(currentHearts); // UI 갱신

        if (currentHearts == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        FadeManager.Instance.FadeOutAndLoad("Dead1"); // Game Over 씬으로 전환
    }
}
