using UnityEngine;
using System.Collections;

public class ObjectShake : MonoBehaviour
{
    public float shakeAmount = 0.05f;  // 흔들림 강도
    public float shakeSpeed = 0.05f;   // 흔들림 주기

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position; // 월드 좌표 기준
        StartCoroutine(ShakeLoop());
    }

    IEnumerator ShakeLoop()
    {
        while (true)
        {
            // 원래 위치 + 랜덤 오프셋
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeAmount, shakeAmount),
                Random.Range(-shakeAmount, shakeAmount),
                0f
            );

            transform.position = originalPosition + randomOffset;

            yield return new WaitForSeconds(shakeSpeed);
        }
    }
}
