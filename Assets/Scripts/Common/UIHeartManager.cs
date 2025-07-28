using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHeartManager : MonoBehaviour
{
    public Image[] hearts; // ��Ʈ �̹��� �迭 (Inspector���� �Ҵ�)
    public float blinkDuration = 0.5f; // �����̴� �� �ð�
    public float blinkInterval = 0.1f; // ������ ����

    public void UpdateHearts(int currentHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = (i < currentHearts);
        }
    }

    public void BlinkHearts(int index)
    {
        if (index >= 0 && index < hearts.Length)
        {
            StartCoroutine(BlinkHeartCoroutine(hearts[index]));
        }
    }

    private IEnumerator BlinkHeartCoroutine(Image heart)
    {
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < blinkDuration)
        {
            visible = !visible;
            heart.enabled = visible;

            elapsed += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        heart.enabled = false; // �������� ������ ����
    }
}
