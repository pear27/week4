using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 전환용

public class Dead : MonoBehaviour
{
    [Header("Fade Settings")]
    public CanvasGroup blackScreen;         // 검은 배경
    public float fadeDuration = 2f;         // 화면이 어두워지는 시간

    [Header("UI Buttons")]
    public CanvasGroup restartButtonCanvas; // 다시하기 버튼 CanvasGroup
    public CanvasGroup exitButtonCanvas;    // 끝내기 버튼 CanvasGroup
    public float buttonFadeDuration = 1f;   // 버튼 페이드인 시간

    private bool isFading = false;          // 페이드 중인지 여부

    void Start()
    {
        // 시작 시 화면과 버튼 숨김
        if (blackScreen != null) blackScreen.alpha = 0f;
        if (restartButtonCanvas != null) restartButtonCanvas.alpha = 0f;
        if (exitButtonCanvas != null) exitButtonCanvas.alpha = 0f;
    }

    void Update()
    {
        // 아무 키나 눌렀을 때 한 번만 페이드 실행
        if (!isFading && Input.anyKeyDown)
        {
            StartCoroutine(FadeSequence());
        }
    }

    private IEnumerator FadeSequence()
    {
        isFading = true;
        yield return StartCoroutine(FadeToBlack());       // 화면 어두워짐
        yield return StartCoroutine(FadeInBothButtons()); // 버튼 2개 동시에 페이드인
    }

    private IEnumerator FadeToBlack()
    {
        if (blackScreen == null) yield break;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            blackScreen.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        blackScreen.alpha = 1f;
    }

    private IEnumerator FadeInBothButtons()
    {
        float elapsed = 0f;
        while (elapsed < buttonFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / buttonFadeDuration);

            if (restartButtonCanvas != null) restartButtonCanvas.alpha = alpha;
            if (exitButtonCanvas != null) exitButtonCanvas.alpha = alpha;

            yield return null;
        }
        if (restartButtonCanvas != null) restartButtonCanvas.alpha = 1f;
        if (exitButtonCanvas != null) exitButtonCanvas.alpha = 1f;
    }

    // 다시하기 버튼
    public void OnRestartButton()
    {
        SceneManager.LoadScene("scene1_slaughter"); // stage2 대신 Farm 씬 로드
    }

    // 끝내기 버튼
    public void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
