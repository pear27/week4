using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;     // �ִ� ��Ʈ ����
    private int currentHearts;    // ���� ��Ʈ ����

    public UIHeartManager uiHeartManager; // ��Ʈ UI�� ������Ʈ�ϴ� �Ŵ��� ����
    public SpriteRenderer spriteRenderer; // �÷��̾��� ��������Ʈ ������

    private Color originalColor; // ���� ��������Ʈ ����

    private void Start()
    {
        currentHearts = maxHearts;
        uiHeartManager.UpdateHearts(currentHearts); // UI �ʱ�ȭ

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color; // ���� ���� ����   
    }

    public void TakeDamage(int amount)
    {
        if (currentHearts <= 0) return; // �̹� ���� ���¸� ����

        int oldHearts = currentHearts; // ���� ��Ʈ ���� ����
        currentHearts -= amount;
        if (currentHearts < 0)
            currentHearts = 0;

        if (currentHearts < oldHearts)
        {
            uiHeartManager.BlinkHearts(currentHearts); // ��Ʈ ������ ȿ��
            Debug.Log($"Player took damage! Current hearts: {currentHearts}");
            StartCoroutine(DamageFlash());
        }

        uiHeartManager.UpdateHearts(currentHearts); // UI ����

        if (currentHearts == 0)
            GameOver();        
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = Color.red; // ���������� ����
        yield return new WaitForSeconds(0.1f); // 0.1�� ���
        spriteRenderer.color = originalColor; // ���� �������� ����
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        FadeManager.Instance.FadeOutAndLoad("Dead1"); // Game Over ������ ��ȯ
    }
}
