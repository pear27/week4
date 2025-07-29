using UnityEngine;
using System.Collections;

public class AudioFadeIn : MonoBehaviour
{
    public AudioSource bgmAudio;    // 재생할 Audio Source
    public float fadeDuration = 10f; // 볼륨이 0→1로 증가하는 시간

    void Start()
    {
        if (bgmAudio != null)
        {
            bgmAudio.volume = 0f;  // 시작 볼륨 0
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
        bgmAudio.volume = 1f; // 최종 볼륨 고정
    }
}
