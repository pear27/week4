using System.Collections;
using UnityEngine;

public class BackgroundBlink : MonoBehaviour
{
    public CanvasGroup bgCanvasGroup; // 배경 CanvasGroup
    public float blinkSpeed = 1.5f;   // 깜빡임 속도
    public float minAlpha = 0.2f;     // 최소 알파 (어두워지는 정도)
    public float maxAlpha = 1f;       // 최대 알파 (밝게)

    void Update()
    {
        float t = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
        bgCanvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, t);
    }
}
