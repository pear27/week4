using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;     // �ִ� ��Ʈ ����
    private int currentHearts;    // ���� ��Ʈ ����

    public UIHeartManager uiHeartManager; // ��Ʈ UI�� ������Ʈ�ϴ� �Ŵ��� ����

    private void Start()
    {
        currentHearts = maxHearts;
        uiHeartManager.UpdateHearts(currentHearts); // UI �ʱ�ȭ
    }

    public void TakeDamage(int amount)
    {
        if (currentHearts <= 0) return; // �̹� ���� ���¸� ����

        int oldHearts = currentHearts; // ���� ��Ʈ ���� ����
        currentHearts -= amount;
        if (currentHearts < 0) currentHearts = 0;

        if (currentHearts < oldHearts)
        {
            uiHeartManager.BlinkHearts(currentHearts); // ��Ʈ ������ ȿ��
            Debug.Log($"Player took damage! Current hearts: {currentHearts}");
        }

        uiHeartManager.UpdateHearts(currentHearts); // UI ����

        if (currentHearts == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        FadeManager.Instance.FadeOutAndLoad("Dead1"); // Game Over ������ ��ȯ
    }
}
