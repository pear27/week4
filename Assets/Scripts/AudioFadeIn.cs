using UnityEngine;
using System.Collections;

public class AudioFadeIn : MonoBehaviour
{
    public AudioSource bgmAudio;    // ����� Audio Source
    public float fadeDuration = 10f; // ������ 0��1�� �����ϴ� �ð�

    void Start()
    {
        if (bgmAudio != null)
        {
            bgmAudio.volume = 0f;  // ���� ���� 0
            bgmAudio.Play();
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            bgmAudio.volume = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        bgmAudio.volume = 1f; // ���� ���� ����
    }
}
