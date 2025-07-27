using System.Collections;
using UnityEngine;

public class BackgroundBlink : MonoBehaviour
{
    public CanvasGroup bgCanvasGroup; // ��� CanvasGroup
    public float blinkSpeed = 1.5f;   // ������ �ӵ�
    public float minAlpha = 0.2f;     // �ּ� ���� (��ο����� ����)
    public float maxAlpha = 1f;       // �ִ� ���� (���)

    void Update()
    {
        float t = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
        bgCanvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, t);
    }
}
