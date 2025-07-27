using UnityEngine;
using System.Collections;

public class ObjectShake : MonoBehaviour
{
    public float shakeAmount = 0.05f;  // ��鸲 ����
    public float shakeSpeed = 0.05f;   // ��鸲 �ֱ�

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position; // ���� ��ǥ ����
        StartCoroutine(ShakeLoop());
    }

    IEnumerator ShakeLoop()
    {
        while (true)
        {
            // ���� ��ġ + ���� ������
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
